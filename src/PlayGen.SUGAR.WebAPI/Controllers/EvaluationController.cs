using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
    // todo replace the skill and achievement controllers with this one and just specify 2 api routes for this class?
    [Authorize("Bearer")]
    public abstract class EvaluationController : Controller
    {
        protected readonly IAuthorizationService _authorizationService;
        protected readonly Core.Controllers.EvaluationController EvaluationCoreController;
        private readonly EvaluationTracker _evaluationTracker;

        protected EvaluationController(Core.Controllers.EvaluationController evaluationCoreController, EvaluationTracker evaluationTracker, IAuthorizationService authorizationService)
        {
            EvaluationCoreController = evaluationCoreController;
            _evaluationTracker = evaluationTracker;
            _authorizationService = authorizationService;
        }

        [Authorization(ClaimScope.Game, AuthorizationOperation.Get, AuthorizationOperation.Achievement)]
        public virtual IActionResult Get(string token, int? gameId)
        {
            if (_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var evaluation = EvaluationCoreController.Get(token, gameId);
                var evaluationContract = evaluation.ToContract();
                return new ObjectResult(evaluationContract);
            }
            return Unauthorized();
        }

        [Authorization(ClaimScope.Game, AuthorizationOperation.Get, AuthorizationOperation.Achievement)]
        public virtual IActionResult Get(int? gameId)
        {
            if (_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var evaluation = EvaluationCoreController.GetByGame(gameId);
                var evaluationContract = evaluation.ToContractList();
                return new ObjectResult(evaluationContract);
            }
            return Unauthorized();
        }
        
        public virtual IActionResult GetGameProgress(int gameId, int? actorId)
        {
            // todo: should this be taken from the progress cache?
            var evaluationsProgress = EvaluationCoreController.GetGameProgress(gameId, actorId);
            var evaluationsProgressResponses = evaluationsProgress.ToContractList();
            return new ObjectResult(evaluationsProgressResponses);
        }
        
        public virtual IActionResult GetEvaluationProgress(string token, int? gameId, int? actorId)
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

        public virtual IActionResult SetSubscribed(int gameId, int actorId, bool subscribed)
        {
            // todo get game and actor id from "session" in HttpContext header
            if (subscribed)
            {
                _evaluationTracker.OnActorSessionStarted(gameId, actorId);
            }
            else
            {
                _evaluationTracker.OnActorSessionEnded(gameId, actorId);
            }

            return new ObjectResult(null);
        }

        [Authorization(ClaimScope.Game, AuthorizationOperation.Delete, AuthorizationOperation.Achievement)]
        public virtual IActionResult Delete(string token, int? gameId)
        {
            if (_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                EvaluationCoreController.Delete(token, gameId);
                return Ok();
            }
            return Unauthorized();
        }
    }
}