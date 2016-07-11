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
	/// Web Controller that facilitates Achievement specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorization]
	public class AchievementsController : Controller
	{
		private readonly Data.EntityFramework.Controllers.AchievementController _achievementController;
		private readonly AchievementController _achievementEvaluationController;

		public AchievementsController(Data.EntityFramework.Controllers.AchievementController achievementController,
			AchievementController achievementEvaluationController)
		{
			_achievementController = achievementController;
			_achievementEvaluationController = achievementEvaluationController;
		}

		/// <summary>
		/// Find an Achievement that matches <param name="token"/> and <param name="gameId"/>.
		/// 
		/// Example Usage: GET api/achievements/find/ACHIEVEMENT_TOKEN/1
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		/// <returns>Returns <see cref="AchievementResponse"/> that holds Achievement details</returns>
		[HttpGet("find/{token}/{gameId:int}")]
		[HttpGet("find/{token}/global")]
		[ResponseType(typeof(AchievementResponse))]
		public IActionResult Get([FromRoute]string token, [FromRoute]int? gameId)
		{
			var achievement = _achievementController.Get(token, gameId);
			var achievementContract = achievement.ToContract();
			return new ObjectResult(achievementContract);
		}

		/// <summary>
		/// Find a list of Achievements that match <param name="gameId"/>.
		/// If global is provided instead of a gameId, get all global achievements, ie. achievements that are not associated with a specific game.
		/// 
		/// Example Usage: GET api/achievements/game/1/list
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Achievement details</returns>
		[HttpGet("global/list")]
		[HttpGet("game/{gameId:int}/list")]
		[ResponseType(typeof(IEnumerable<AchievementResponse>))]
		public IActionResult Get([FromRoute]int? gameId)
		{
			var achievement = _achievementController.GetByGame(gameId);
			var achievementContract = achievement.ToContractList();
			return new ObjectResult(achievementContract);
		}

		/// <summary>
		/// Find the current progress for all achievements for a <param name="gameId"/> for <param name="actorId"/>.
		/// 
		/// Example Usage: GET api/achievements/game/1/evaluate/1
		/// </summary>
		/// <param name="gameId">ID of Game</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current progress toward achievement.</returns>
		[HttpGet("game/{gameId:int}/evaluate")]
		[HttpGet("global/evaluate")]
		[HttpGet("game/{gameId:int}/evaluate/{actorId:int}")]
		[HttpGet("global/evaluate/{actorId:int}")]
		[ResponseType(typeof(IEnumerable<AchievementProgressResponse>))]
		public IActionResult GetGameProgress([FromRoute]int gameId, [FromRoute]int? actorId)
		{
			var achievements = _achievementController.GetByGame(gameId);
			achievements = _achievementEvaluationController.FilterByActorType(achievements, actorId);
			var achievementResponses = achievements.Select(a =>
			{
				var completed = _achievementEvaluationController.IsAchievementCompleted(a, actorId);
				return new AchievementProgressResponse
				{
					Name = a.Name,
					Progress = completed,
				};
			});

			return new ObjectResult(achievementResponses);
		}

		/// <summary>
		/// Find the current progress for an Achievement for <param name="actorId"/>.
		/// 
		/// Example Usage: GET api/achievements/ACHIEVEMENT_TOKEN/1/evaluate/1
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current progress toward achievement.</returns>
		[HttpGet("{token}/{gameId:int}/evaluate")]
		[HttpGet("{token}/global/evaluate")]
		[HttpGet("{token}/{gameId:int}/evaluate/{actorId:int}")]
		[HttpGet("{token}/global/evaluate/{actorId:int}")]
		[ResponseType(typeof(AchievementProgressResponse))]
		public IActionResult GetAchievementProgress([FromRoute]string token, [FromRoute]int? gameId, [FromRoute]int? actorId)
		{
			var achievement = _achievementController.Get(token, gameId);
			var completed = _achievementEvaluationController.IsAchievementCompleted(achievement, actorId);
			return new ObjectResult(new AchievementProgressResponse
			{
				Name = achievement.Name,
				Progress = completed,
			});
		}

		/// <summary>
		/// Create a new Achievement.
		/// Requires <see cref="AchievementRequest.Name"/> to be unique to that <see cref="AchievementRequest.GameId"/>.
		/// 
		/// Example Usage: POST api/achievements/create
		/// </summary>
		/// <param name="newAchievement"><see cref="AchievementRequest"/> object that holds the details of the new Achievement.</param>
		/// <returns>Returns a <see cref="AchievementResponse"/> object containing details for the newly created Achievement.</returns>
		[HttpPost("create")]
		[ResponseType(typeof(AchievementResponse))]
		[ArgumentsNotNull]
		public IActionResult Create([FromBody] AchievementRequest newAchievement)
		{
			var achievement = newAchievement.ToAchievementModel();
			_achievementController.Create(achievement);
			var achievementContract = achievement.ToContract();
			return new ObjectResult(achievementContract);
		}

		/// <summary>
		/// Update an existing Achievement.
		/// 
		/// Example Usage: PUT api/achievements/update
		/// </summary>
		/// <param name="achievement"><see cref="AchievementRequest"/> object that holds the details of the Achievement.</param>
		[HttpPut("update")]
		[ArgumentsNotNull]
		public void Update([FromBody] AchievementRequest achievement)
		{
			var achievementModel = achievement.ToAchievementModel();
			_achievementController.Update(achievementModel);
		}

		/// <summary>
		/// Delete Achievement with the <param name="token"/> and <param name="gameId"/> provided.
		/// 
		/// Example Usage: DELETE api/achievements/ACHIEVEMENT_TOKEN/1
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		[HttpDelete("{token}/global")]
		[HttpDelete("{token}/{gameId:int}")]
		public void Delete([FromRoute]string token, [FromRoute]int? gameId)
		{
			_achievementController.Delete(token, gameId);
		}

	}
}