using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;

using Microsoft.AspNetCore.Mvc;

using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Exceptions;
using PlayGen.SUGAR.GameData;
using PlayGen.SUGAR.WebAPI.Controllers.Filters;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Leaderboard specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorization]
	public class LeaderboardsController : Controller
	{
		private readonly Data.EntityFramework.Controllers.LeaderboardController _leaderboardController;
		private readonly LeaderboardController _leaderboardEvaluationController;

		public LeaderboardsController(Data.EntityFramework.Controllers.LeaderboardController leaderboardController,
			LeaderboardController leaderboardEvaluationController)
		{
			_leaderboardController = leaderboardController;
			_leaderboardEvaluationController = leaderboardEvaluationController;
		}

		/// <summary>
		/// Find a list of leaderboards that match <param name="gameId"/>.
		/// If global is provided instead of a gameId, get all global leaderboards, ie. leaderboards that are not associated with a specific game.
		/// 
		/// Example Usage: GET api/leaderboards/game/1/list
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="LeaderboardResponse"/> that hold Leaderboard details</returns>
		[HttpGet("global/list")]
		[HttpGet("game/{gameId:int}/list")]
		[ResponseType(typeof(IEnumerable<LeaderboardResponse>))]
		public IActionResult Get([FromRoute]int? gameId)
		{
			var leaderboard = _leaderboardController.GetByGame(gameId);
			var leaderboardContract = leaderboard.ToContractList();
			return new ObjectResult(leaderboardContract);
		}

		/// <summary>
		/// Create a new Leaderboard.
		/// Requires <see cref="LeaderboardRequest.Name"/> and <see cref="LeaderboardRequest.Token"/> to be unique to that <see cref="LeaderboardRequest.GameId"/>.
		/// 
		/// Example Usage: POST api/leaderboards/create
		/// </summary>
		/// <param name="newLeaderboard"><see cref="LeaderboardRequest"/> object that holds the details of the new Leaderboard.</param>
		/// <returns>Returns a <see cref="LeaderboardResponse"/> object containing details for the newly created Leaderboard.</returns>
		[HttpPost("create")]
		[ResponseType(typeof(LeaderboardResponse))]
		[ArgumentsNotNull]
		public IActionResult Create([FromBody] LeaderboardRequest newLeaderboard)
		{
			var leaderboard = newLeaderboard.ToModel();
			_leaderboardController.Create(leaderboard);
			var leaderboardContract = leaderboard.ToContract();
			return new ObjectResult(leaderboardContract);
		}

		/// <summary>
		/// Get the standings for a Leaderboard using a <see cref="LeaderboardStandingRequest"/>.
		/// 
		/// Example Usage: POST api/leaderboards/standings
		/// </summary>
		/// <param name="leaderboardDetails"><see cref="LeaderboardStandingsRequest"/> object that holds the details that are wanted from the Leaderboard.</param>
		/// <returns>Returns multiple <see cref="LeaderboardStandingsResponse"/> that hold actor positions in the leaderboard.</returns>
		[HttpPost("standings")]
		[ResponseType(typeof(IEnumerable<LeaderboardStandingsResponse>))]
		public IActionResult GetLeaderboardStandings([FromBody]LeaderboardStandingsRequest leaderboardDetails)
		{
			var leaderboard = _leaderboardController.Get(leaderboardDetails.LeaderboardToken, leaderboardDetails.GameId);
			var standings = _leaderboardEvaluationController.GetStandings(leaderboard, leaderboardDetails);
			return new ObjectResult(standings);
		}

		/// <summary>
		/// Update an existing Leaderboard.
		/// 
		/// Example Usage: PUT api/leaderboards/update
		/// </summary>
		/// <param name="leaderboard"><see cref="LeaderboardRequest"/> object that holds the details of the Leaderboard.</param>
		[HttpPut("update")]
		[ArgumentsNotNull]
		public void Update([FromBody] LeaderboardRequest leaderboard)
		{
			var leaderboardModel = leaderboard.ToModel();
			_leaderboardController.Update(leaderboardModel);
		}

		/// <summary>
		/// Delete Leaderboard with the <param name="token"/> and <param name="gameId"/> provided.
		/// 
		/// Example Usage: DELETE api/leaderboards/LEADERBOARD_TOKEN/1
		/// </summary>
		/// <param name="token">Token of Leaderboard</param>
		/// <param name="gameId">ID of the Game the Leaderboard is for</param>
		[HttpDelete("{token}/global")]
		[HttpDelete("{token}/{gameId:int}")]
		public void Delete([FromRoute]string token, [FromRoute]int? gameId)
		{
			_leaderboardController.Delete(token, gameId);
		}
	}
}