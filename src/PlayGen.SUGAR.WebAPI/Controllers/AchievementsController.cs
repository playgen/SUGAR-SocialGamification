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
		/// Get all global achievements, ie. achievements that are not associated with a specific game
		/// 
		/// Example Usage: GET api/achievements/list
		/// </summary>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Achievement details</returns>
		[HttpGet("list")]
		[ResponseType(typeof(IEnumerable<AchievementResponse>))]
		public IActionResult Get()
		{
			var achievement = _achievementController.GetGlobal();
			var achievementContract = achievement.ToContractList();
			return new ObjectResult(achievementContract);
		}

		/// <summary>
		/// Find a list of Achievements that match <param name="gameId"/>.
		/// 
		/// Example Usage: GET api/achievements/game/1/list
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Achievement details</returns>
		[HttpGet("game/{gameId:int}/list")]
		[ResponseType(typeof(IEnumerable<AchievementResponse>))]
		public IActionResult Get([FromRoute]int gameId)
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
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current group progress toward achievement.</returns>
		[HttpGet("game/{gameId:int}/evaluate")]
		[HttpGet("game/{gameId:int}/evaluate/{actorId:int}")]
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
					Progress = completed ? 1 : 0,
				};
			});

			return new ObjectResult(achievementResponses);
		}

		/// <summary>
		/// Find the current progress for an <param name="achievementId"/> for <param name="actorId"/>.
		/// 
		/// Example Usage: GET api/achievements/1/evaluate/1
		/// </summary>
		/// <param name="achievementId">ID of Achievement</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current group progress toward achievement.</returns>
		[HttpGet("{achievementId:int}/evaluate")]
		[HttpGet("{achievementId:int}/evaluate/{actorId:int}")]
		[ResponseType(typeof(AchievementProgressResponse))]
		public IActionResult GetAchievementProgress([FromRoute]int achievementId, [FromRoute]int? actorId)
		{
			var achievement = _achievementController.Get(achievementId);
			var completed = _achievementEvaluationController.IsAchievementCompleted(achievement, actorId);
			return new ObjectResult(new AchievementProgressResponse
			{
				Name = achievement.Name,
				Progress = completed ? 1 : 0,
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
		/// Example Usage: PUT api/achievements/update/1
		/// </summary>
		/// <param name="id">Id of the existing Achievement.</param>
		/// <param name="achievement"><see cref="AchievementRequest"/> object that holds the details of the Achievement.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
		public void Update([FromRoute] int id, [FromBody] AchievementRequest achievement)
		{
			var achievementModel = achievement.ToAchievementModel();
			achievementModel.Id = id;
			_achievementController.Update(achievementModel);
		}


		/// <summary>
		/// Delete Achievements with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/achievements/1
		/// </summary>
		/// <param name="id">Achievement ID</param>
		[HttpDelete("{id:int}")]
		public void Delete([FromRoute]int id)
		{
			_achievementController.Delete(id);
		}

	}
}