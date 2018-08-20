using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.Model;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Achievement specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	public class AchievementsController : EvaluationsController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="evaluationCoreController"></param>
		/// <param name="evaluationTracker"></param>
		/// <param name="authorizationService"></param>
		public AchievementsController(EvaluationController evaluationCoreController, EvaluationTracker evaluationTracker, IAuthorizationService authorizationService) : base(evaluationCoreController, evaluationTracker, authorizationService)
		{
		}

		/// <summary>
		/// Find an Achievement that matches the token and gameId provided.
		/// If global is provided instead of a gameId, get the global Achievement that is not associated with a specific game with the provided token.
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		/// <returns>Returns <see cref="EvaluationResponse"/> that holds Achievement details</returns>
		[HttpGet("find/{token}/{gameId:int}")]
		[HttpGet("find/{token}/global")]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.Achievement)]
		public Task<IActionResult> Get([FromRoute]string token, [FromRoute]int gameId)
		{
			return Get(token, gameId, EvaluationType.Achievement);
		}

		/// <summary>
		/// Find a list of Achievements for the gameId provided.
		/// If global is provided instead of a gameId, get all global achievements, ie. achievements that are not associated with a specific game.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="EvaluationResponse"/> that hold Achievement details</returns>
		[HttpGet("global/list")]
		[HttpGet("game/{gameId:int}/list")]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.Achievement)]
		public Task<IActionResult> Get([FromRoute]int gameId)
		{
			return Get(gameId, EvaluationType.Achievement);
		}

		/// <summary>
		/// Find the current progress for all achievements for a gameId for actorId.
		/// If global is provided instead of a gameId, get all progress for global achievements, ie. achievements that are not associated with a specific game.
		/// </summary>
		/// <param name="gameId">ID of Game</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="EvaluationProgressResponse"/> that hold current progress toward achievements.</returns>
		[HttpGet("game/{gameId:int}/evaluate/{actorId:int}")]
		[HttpGet("global/evaluate/{actorId:int}")]
		public IActionResult GetGameProgress([FromRoute]int gameId, [FromRoute]int actorId)
		{
			return GetGameProgress(gameId, actorId, EvaluationType.Achievement);
		}

		/// <summary>
		/// Find the current progress for the Achievement with the provided token and gameId for the actorId.
		/// If global is provided instead of a gameId, get progress for a global achievement with the token provided, ie. an achievement that is not associated with a specific game.
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns a <see cref="EvaluationProgressResponse"/> that hold current progress toward the achievement.</returns>
		[HttpGet("{token}/{gameId:int}/evaluate/{actorId:int}")]
		[HttpGet("{token}/global/evaluate/{actorId:int}")]
		public IActionResult GetAchievementProgress([FromRoute]string token, [FromRoute]int gameId, [FromRoute]int actorId)
		{
			return GetEvaluationProgress(token, gameId, actorId, EvaluationType.Achievement);
		}

		/// <summary>
		/// Create a new Achievement.
		/// Requires <see cref="EvaluationCreateRequest.Name"/> to be unique to that <see cref="EvaluationCreateRequest.GameId"/>.
		/// </summary>
		/// <param name="newAchievement"><see cref="EvaluationCreateRequest"/> object that holds the details of the new Achievement.</param>
		/// <returns>Returns a <see cref="EvaluationResponse"/> object containing details for the newly created Achievement.</returns>
		[HttpPost("create")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Game, AuthorizationAction.Create, AuthorizationEntity.Achievement)]
		public async Task<IActionResult> Create([FromBody] EvaluationCreateRequest newAchievement)
		{
			if ((await _authorizationService.AuthorizeAsync(User, newAchievement.GameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var achievement = newAchievement.ToAchievementModel();
				achievement = (Achievement)EvaluationCoreController.Create(achievement);
				var achievementContract = achievement.ToContract();
				return new ObjectResult(achievementContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Update an existing Achievement.
		/// </summary>
		/// <param name="achievement"><see cref="EvaluationCreateRequest"/> object that holds the details of the Achievement.</param>
		[HttpPut("update")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Game, AuthorizationAction.Update, AuthorizationEntity.Achievement)]
		public async Task<IActionResult> Update([FromBody] EvaluationUpdateRequest achievement)
		{
			if ((await _authorizationService.AuthorizeAsync(User, achievement.GameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var achievementModel = achievement.ToAchievementModel();
				EvaluationCoreController.Update(achievementModel);
				return Ok();
			}
			return Forbid();
		}

		/// <summary>
		/// Delete the Achievement with the token and gameId provided.
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		[HttpDelete("{token}/global")]
		[HttpDelete("{token}/{gameId:int}")]
		[Authorization(ClaimScope.Game, AuthorizationAction.Delete, AuthorizationEntity.Achievement)]
		public Task<IActionResult> Delete([FromRoute]string token, [FromRoute]int gameId)
		{
			return Delete(token, gameId, EvaluationType.Achievement);
		}
	}
}