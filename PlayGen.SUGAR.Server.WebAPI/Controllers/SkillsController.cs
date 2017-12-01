using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.Model;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Skill specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class SkillsController : EvaluationsController
	{
		public SkillsController(Core.Controllers.EvaluationController evaluationCoreController, EvaluationTracker evaluationTracker, IAuthorizationService authorizationService)
			: base(evaluationCoreController, evaluationTracker, authorizationService)
		{
		}

		/// <summary>
		/// Find a Skill that matches <param name="token"/> and <param name="gameId"/>.
		/// 
		/// Example Usage: GET api/skills/find/SKILL_TOKEN/1
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		/// <returns>Returns <see cref="EvaluationResponse"/> that holds Skill details</returns>
		[HttpGet("find/{token}/{gameId:int}")]
		[HttpGet("find/{token}/global")]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.Achievement)]
		public new Task<IActionResult> Get([FromRoute]string token, [FromRoute]int? gameId)
		{
			return base.Get(token, gameId);
		}

		/// <summary>
		/// Find a list of Skills that match <param name="gameId"/>.
		/// If global is provided instead of a gameId, get all global skills, ie. skills that are not associated with a specific game.
		/// 
		/// Example Usage: GET api/skills/game/1/list
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="EvaluationResponse"/> that hold Skill details</returns>
		[HttpGet("global/list")]
		[HttpGet("game/{gameId:int}/list")]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.Achievement)]
		public Task<IActionResult> Get([FromRoute]int? gameId)
		{
			return Get(gameId, EvaluationType.Skill);
		}

		/// <summary>
		/// Find the current progress for all skills for a <param name="gameId"/> for <param name="actorId"/>.
		/// 
		/// Example Usage: GET api/skills/game/1/evaluate/1
		/// </summary>
		/// <param name="gameId">ID of Game</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="EvaluationProgressResponse"/> that hold current progress toward skill.</returns>
		[HttpGet("game/{gameId:int}/evaluate")]
		[HttpGet("global/evaluate")]
		[HttpGet("game/{gameId:int}/evaluate/{actorId:int}")]
		[HttpGet("global/evaluate/{actorId:int}")]
		public new IActionResult GetGameProgress([FromRoute]int gameId, [FromRoute]int? actorId)
		{
			return base.GetGameProgress(gameId, actorId);
		}

		/// <summary>
		/// Find the current progress for a Skill for <param name="actorId"/>.
		/// 
		/// Example Usage: GET api/skills/SKILL_TOKEN/1/evaluate/1
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="EvaluationProgressResponse"/> that hold current progress toward skill.</returns>
		[HttpGet("{token}/{gameId:int}/evaluate")]
		[HttpGet("{token}/global/evaluate")]
		[HttpGet("{token}/{gameId:int}/evaluate/{actorId:int}")]
		[HttpGet("{token}/global/evaluate/{actorId:int}")]
		public IActionResult GetSkillProgress([FromRoute]string token, [FromRoute]int? gameId, [FromRoute]int? actorId)
		{
			return GetEvaluationProgress(token, gameId, actorId);
		}

		/// <summary>
		/// Create a new Skill.
		/// Requires <see cref="EvaluationCreateRequest.Name"/> to be unique to that <see cref="EvaluationCreateRequest.GameId"/>.
		/// 
		/// Example Usage: POST api/skills/create
		/// </summary>
		/// <param name="newSkill"><see cref="EvaluationCreateRequest"/> object that holds the details of the new Skill.</param>
		/// <returns>Returns a <see cref="EvaluationResponse"/> object containing details for the newly created Skill.</returns>
		[HttpPost("create")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Game, AuthorizationAction.Create, AuthorizationEntity.Achievement)]
		public async Task<IActionResult> Create([FromBody] EvaluationCreateRequest newSkill)
		{
			if (await _authorizationService.AuthorizeAsync(User, newSkill.GameId, HttpContext.ScopeItems(ClaimScope.Game)))
			{
				var skill = newSkill.ToSkillModel();
				skill = (Skill)EvaluationCoreController.Create(skill);
				var achievementContract = skill.ToContract();
				return new ObjectResult(achievementContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Update an existing Skill.
		/// 
		/// Example Usage: PUT api/skills/update
		/// </summary>
		/// <param name="skill"><see cref="EvaluationCreateRequest"/> object that holds the details of the Skill.</param>
		[HttpPut("update")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Game, AuthorizationAction.Update, AuthorizationEntity.Achievement)]
		public async Task<IActionResult> Update([FromBody] EvaluationUpdateRequest skill)
		{
			if (await _authorizationService.AuthorizeAsync(User, skill.GameId, HttpContext.ScopeItems(ClaimScope.Game)))
			{
				var skillModel = skill.ToSkillModel();
				EvaluationCoreController.Update(skillModel);
				return Ok();
			}
			return Forbid();
		}

		/// <summary>
		/// Delete Skill with the <param name="token"/> and <param name="gameId"/> provided.
		/// 
		/// Example Usage: DELETE api/skills/SKILL_TOKEN/1
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		[HttpDelete("{token}/global")]
		[HttpDelete("{token}/{gameId:int}")]
		[Authorization(ClaimScope.Game, AuthorizationAction.Delete, AuthorizationEntity.Achievement)]
		public new Task<IActionResult> Delete([FromRoute]string token, [FromRoute]int? gameId)
		{
			return base.Delete(token, gameId);
		}
	}
}