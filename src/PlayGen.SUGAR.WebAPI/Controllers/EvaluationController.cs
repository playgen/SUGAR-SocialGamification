using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
    // todo replace the skill and achievement controllers with this one and just specify 2 api routes for this class?
    public abstract class EvaluationController : Controller
    {
        protected readonly Core.Controllers.EvaluationController EvaluationCoreController;
        private readonly EvaluationTracker _evaluationTracker;

        protected EvaluationController(Core.Controllers.EvaluationController evaluationCoreController, EvaluationTracker evaluationTracker)
        {
            EvaluationCoreController = evaluationCoreController;
            _evaluationTracker = evaluationTracker;
        }
        
        public virtual IActionResult Get(string token, int? gameId)
        {
            var evaluation = EvaluationCoreController.Get(token, gameId);
            var evaluationContract = evaluation.ToContract();
            return new ObjectResult(evaluationContract);
        }
        
        public virtual IActionResult Get(int? gameId)
        {
            var evaluation = EvaluationCoreController.GetByGame(gameId);
            var evaluationContract = evaluation.ToContractList();
            return new ObjectResult(evaluationContract);
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

        public virtual void Delete(string token, int? gameId)
        {
            EvaluationCoreController.Delete(token, gameId);
        }
    }
}