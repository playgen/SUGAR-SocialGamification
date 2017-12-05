using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	// todo replace the skill and achievement controllers with this one and just specify 2 api routes for this class?
	public abstract class EvaluationsController : Controller
	{
		protected readonly IAuthorizationService _authorizationService;
		protected readonly Core.Controllers.EvaluationController EvaluationCoreController;
		private readonly EvaluationTracker _evaluationTracker;

		protected EvaluationsController(Core.Controllers.EvaluationController evaluationCoreController, EvaluationTracker evaluationTracker, IAuthorizationService authorizationService)
		{
			EvaluationCoreController = evaluationCoreController;
			_evaluationTracker = evaluationTracker;
			_authorizationService = authorizationService;
		}

		protected async Task<IActionResult> Get(int gameId, EvaluationType evaluationType)
		{
			if ((await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var evaluation = EvaluationCoreController.GetEvaluation(gameId, evaluationType);
				var evaluationContract = evaluation.ToContractList();
				return new ObjectResult(evaluationContract);
			}
			return Forbid();
		}

		protected async Task<IActionResult> Get(string token, int gameId)
		{
			if ((await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var evaluation = EvaluationCoreController.Get(token, gameId);
				var evaluationContract = evaluation.ToContract();
				return new ObjectResult(evaluationContract);
			}
			return Forbid();
		}

		protected IActionResult GetGameProgress(int gameId, int actorId)
		{
			// todo: should this be taken from the progress cache?
			var evaluationsProgress = EvaluationCoreController.GetGameProgress(gameId, actorId);
			var evaluationsProgressResponses = evaluationsProgress.ToContractList();
			return new ObjectResult(evaluationsProgressResponses);
		}

		protected IActionResult GetEvaluationProgress(string token, int gameId, int actorId)
		{
			// todo: should this be taken from the progress cache?
			var evaluation = EvaluationCoreController.Get(token, gameId);
			var progress = EvaluationCoreController.EvaluateProgress(evaluation, actorId);
			return new ObjectResult(new EvaluationProgressResponse {
				Name = evaluation.Name,
				Progress = progress
			});
		}

		protected async Task<IActionResult> Delete(string token, int gameId)
		{
			if ((await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				EvaluationCoreController.Delete(token, gameId);
				return Ok();
			}
			return Forbid();
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member