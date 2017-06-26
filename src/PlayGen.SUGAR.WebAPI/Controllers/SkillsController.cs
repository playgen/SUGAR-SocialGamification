using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Permissions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.WebAPI.Attributes;

namespace PlayGen.SUGAR.WebAPI.Controllers
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
		//[ResponseType(typeof(EvaluationResponse))]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Get, AuthorizationOperation.Achievement)]
		public new IActionResult Get([FromRoute]string token, [FromRoute]int? gameId)
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
		//[ResponseType(typeof(IEnumerable<EvaluationResponse>))]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Get, AuthorizationOperation.Achievement)]
		public new IActionResult Get([FromRoute]int? gameId)
		{
			return base.Get(gameId);
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
		//[ResponseType(typeof(IEnumerable<EvaluationProgressResponse>))]
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
		//[ResponseType(typeof(EvaluationProgressResponse))]
		public IActionResult GetSkillProgress([FromRoute]string token, [FromRoute]int? gameId, [FromRoute]int? actorId)
		{
			return base.GetEvaluationProgress(token, gameId, actorId);
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
		//[ResponseType(typeof(EvaluationResponse))]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.Achievement)]
		public IActionResult Create([FromBody] EvaluationCreateRequest newSkill)
		{
			if (_authorizationService.AuthorizeAsync(User, newSkill.GameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
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
		[Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Achievement)]
		public IActionResult Update([FromBody] EvaluationUpdateRequest skill)
		{
			if (_authorizationService.AuthorizeAsync(User, skill.GameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
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
		[Authorization(ClaimScope.Game, AuthorizationOperation.Delete, AuthorizationOperation.Achievement)]
		public new IActionResult Delete([FromRoute]string token, [FromRoute]int? gameId)
		{
			return base.Delete(token, gameId);
		}
	}
}