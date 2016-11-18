using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Core.Controllers;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.Core.Utilities;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Achievement specific operations.
	/// </summary>
	[Route("api/[controller]")]    
    public class AchievementsController : EvaluationController
    {
        private readonly EvaluationController _evaluationCoreController;

        public AchievementsController(Core.Controllers.EvaluationController evaluationCoreController, EvaluationTracker evaluationTracker, IAuthorizationService authorizationService)
            : base(evaluationCoreController, evaluationTracker, authorizationService)
        {
        }
		/// <summary>
		/// Find an Achievement that matches <param name="token"/> and <param name="gameId"/>.
		/// 
		/// Example Usage: GET api/achievements/find/ACHIEVEMENT_TOKEN/1
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		/// <returns>Returns <see cref="EvaluationResponse"/> that holds Achievement details</returns>
		[HttpGet("find/{token}/{gameId:int}")]
		[HttpGet("find/{token}/global")]
        //[ResponseType(typeof(EvaluationResponse))]
        public override IActionResult Get([FromRoute]string token, [FromRoute]int? gameId)
		{
            return base.Get(token, gameId);
        }

		/// <summary>
		/// Find a list of Achievements that match <param name="gameId"/>.
		/// If global is provided instead of a gameId, get all global achievements, ie. achievements that are not associated with a specific game.
		/// 
		/// Example Usage: GET api/achievements/game/1/list
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="EvaluationResponse"/> that hold Achievement details</returns>
		[HttpGet("global/list")]
		[HttpGet("game/{gameId:int}/list")]
        //[ResponseType(typeof(IEnumerable<EvaluationResponse>))]
        public override IActionResult Get([FromRoute]int? gameId)
        {
            return base.Get(gameId);
        }

        /// <summary>
        /// Find the current progress for all achievements for a <param name="gameId"/> for <param name="actorId"/>.
        /// 
        /// Example Usage: GET api/achievements/game/1/evaluate/1
        /// </summary>
        /// <param name="gameId">ID of Game</param>
        /// <param name="actorId">ID of Group/User</param>
        /// <returns>Returns multiple <see cref="EvaluationProgressResponse"/> that hold current progress toward achievement.</returns>
        [HttpGet("game/{gameId:int}/evaluate")]
		[HttpGet("global/evaluate")]
		[HttpGet("game/{gameId:int}/evaluate/{actorId:int}")]
		[HttpGet("global/evaluate/{actorId:int}")]
		//[ResponseType(typeof(IEnumerable<EvaluationProgressResponse>))]
		public override IActionResult GetGameProgress([FromRoute]int gameId, [FromRoute]int? actorId)
		{
		    return base.GetGameProgress(gameId, actorId);
		}

		/// <summary>
		/// Find the current progress for an Achievement for <param name="actorId"/>.
		/// 
		/// Example Usage: GET api/achievements/ACHIEVEMENT_TOKEN/1/evaluate/1
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="EvaluationProgressResponse"/> that hold current progress toward achievement.</returns>
		[HttpGet("{token}/{gameId:int}/evaluate")]
		[HttpGet("{token}/global/evaluate")]
		[HttpGet("{token}/{gameId:int}/evaluate/{actorId:int}")]
		[HttpGet("{token}/global/evaluate/{actorId:int}")]
		//[ResponseType(typeof(EvaluationProgressResponse))]
		public IActionResult GetAchievementProgress([FromRoute]string token, [FromRoute]int? gameId, [FromRoute]int? actorId)
		{
		    return base.GetEvaluationProgress(token, gameId, actorId);
		}

        /// <summary>
        /// Subscribe the current user for the current game to revieve notifications when achievements
        /// have been completed.
        /// 
        /// Example Usage: POST api/achievements/true
        /// </summary>
        /// <param name="gameId">The game to send events for.</param>
        /// <param name="actorId">The actor (user or group) to send events for.</param>
        /// <param name="subscribed">Boolean value whether to subscribe or not.</param>
        /// <returns>Any pending events will be attached to the response.</returns>
        [HttpPost("setsubscribed/{subscribed}")]
        public override IActionResult SetSubscribed(int gameId, int actorId, bool subscribed)
        {
            return base.SetSubscribed(gameId, actorId, subscribed);
        }

        /// <summary>
        /// Create a new Achievement.
        /// Requires <see cref="EvaluationRequest.Name"/> to be unique to that <see cref="EvaluationRequest.GameId"/>.
        /// 
        /// Example Usage: POST api/achievements/create
        /// </summary>
        /// <param name="newAchievement"><see cref="EvaluationRequest"/> object that holds the details of the new Achievement.</param>
        /// <returns>Returns a <see cref="EvaluationResponse"/> object containing details for the newly created Achievement.</returns>
        [HttpPost("create")]
		//[ResponseType(typeof(EvaluationResponse))]
		[ArgumentsNotNull]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.Achievement)]
        public IActionResult Create([FromBody] EvaluationCreateRequest newAchievement)
		{
            if (_authorizationService.AuthorizeAsync(User, newAchievement.GameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var achievement = newAchievement.ToAchievementModel();
                achievement = (Achievement)EvaluationCoreController.Create(achievement);
                var achievementContract = achievement.ToContract();
                return new ObjectResult(achievementContract);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Update an existing Achievement.
        /// 
        /// Example Usage: PUT api/achievements/update
        /// </summary>
        /// <param name="achievement"><see cref="EvaluationCreateRequest"/> object that holds the details of the Achievement.</param>
        [HttpPut("update")]
		[ArgumentsNotNull]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Achievement)]
        public IActionResult Update([FromBody] EvaluationUpdateRequest achievement)
		{
            if (_authorizationService.AuthorizeAsync(User, achievement.GameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var achievementModel = achievement.ToAchievementModel();
                EvaluationCoreController.Update(achievementModel);
                return Ok();
            }
            return Unauthorized();
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
		public override IActionResult Delete([FromRoute]string token, [FromRoute]int? gameId)
		{
            return base.Delete(token, gameId);
		}
	}
}