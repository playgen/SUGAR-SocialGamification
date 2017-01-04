using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
    //TODO: replace the skill and achievement controllers with this one and just specify 2 api routes for this class?
    /// <summary>
    /// Base controller for entities that require evaluation data processing
    /// </summary>
    public abstract class EvaluationsController : AuthorizedController
    {
        protected readonly Core.Controllers.EvaluationController EvaluationCoreController;
        private readonly EvaluationTracker _evaluationTracker;

        /// <summary>
        /// TODO: fill this in
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="evaluationCoreController"></param>
        /// <param name="evaluationTracker"></param>
        protected EvaluationsController(IAuthorizationService authorizationService, Core.Controllers.EvaluationController evaluationCoreController, EvaluationTracker evaluationTracker)
			: base (authorizationService)
        {
            EvaluationCoreController = evaluationCoreController;
            _evaluationTracker = evaluationTracker;
        }
        
        protected IActionResult Get(string token, int gameId)
        {
            var evaluation = EvaluationCoreController.Get(token, gameId);
            var evaluationContract = evaluation.ToContract();
            return new ObjectResult(evaluationContract);
        }
        
        protected IActionResult Get(int gameId)
        {
            if (AuthorizedGame(gameId))
            {
                var evaluation = EvaluationCoreController.GetByGame(gameId);
                var evaluationContract = evaluation.ToContractList();
                return new ObjectResult(evaluationContract);
            }
            return Forbid();
        }

        protected IActionResult GetGameProgress(int gameId, int? actorId)
        {
            // todo: should this be taken from the progress cache?
            var evaluationsProgress = EvaluationCoreController.GetGameProgress(gameId, actorId);
            var evaluationsProgressResponses = evaluationsProgress.ToContractList();
            return new ObjectResult(evaluationsProgressResponses);
        }

        protected IActionResult GetEvaluationProgress(string token, int? gameId, int? actorId)
        {
            // todo: should this be taken from the progress cache?
            var evaluation = EvaluationCoreController.Get(token, gameId);
            var progress = EvaluationCoreController.EvaluateProgress(evaluation, actorId);
            return new ObjectResult(new EvaluationProgressResponse
            {
                Name = evaluation.Name,
                Progress = progress,
            });
        }
        
        protected IActionResult Delete(string token, int? gameId)
        {
            if (_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                EvaluationCoreController.Delete(token, gameId);
                return Ok();
            }
            return Forbid();
        }
    }
}