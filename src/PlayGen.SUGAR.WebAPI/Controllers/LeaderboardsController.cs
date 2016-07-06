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
		/// Get all global leaderboards, ie. leaderboards that are not associated with a specific game
		/// 
		/// Example Usage: GET api/leaderboards/list
		/// </summary>
		/// <returns>Returns multiple <see cref="LeaderboardResponse"/> that hold Leaderboard details</returns>
		[HttpGet("list")]
		[ResponseType(typeof(IEnumerable<LeaderboardResponse>))]
		public IActionResult Get()
		{
			var leaderboard = _leaderboardController.GetGlobal();
			var leaderboardContract = leaderboard.ToContractList();
			return new ObjectResult(leaderboardContract);
		}

		/// <summary>
		/// Find a list of leaderboards that match <param name="gameId"/>.
		/// 
		/// Example Usage: GET api/leaderboards/game/1/list
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="LeaderboardResponse"/> that hold Leaderboard details</returns>
		[HttpGet("game/{gameId:int}/list")]
		[ResponseType(typeof(IEnumerable<LeaderboardResponse>))]
		public IActionResult Get([FromRoute]int gameId)
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
			var leaderboard = _leaderboardController.Get(leaderboardDetails.LeaderboardId);
			var standings = _leaderboardEvaluationController.GetStandings(leaderboard, leaderboardDetails);
			return new ObjectResult(standings);
		}

		/// <summary>
		/// Delete Leaderboards with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/leaderboards/1
		/// </summary>
		/// <param name="id">Leaderboard ID</param>
		[HttpDelete("{id:int}")]
		public void Delete([FromRoute]int id)
		{
			_leaderboardController.Delete(id);
		}
	}
}