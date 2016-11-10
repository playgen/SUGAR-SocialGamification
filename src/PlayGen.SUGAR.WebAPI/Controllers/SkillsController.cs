using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Core;
using PlayGen.SUGAR.Core.Controllers;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Skill specific operations.
	/// </summary>
	[Route("api/[controller]")]
		[Authorization]
		public class SkillsController : Controller
	{
		private readonly Data.EntityFramework.Controllers.EvaluationController _evaluationDbController;
		private readonly EvaluationController _evaluationController;

		public SkillsController(Data.EntityFramework.Controllers.EvaluationController evaluationDbController,
			EvaluationController evaluationController)
		{
			_evaluationDbController = evaluationDbController;
			_evaluationController = evaluationController;
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
		public IActionResult Get([FromRoute]string token, [FromRoute]int? gameId)
		{
			var skill = _evaluationDbController.Get(token, gameId);
			var skillContract = skill.ToContract();
			return new ObjectResult(skillContract);
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
		public IActionResult Get([FromRoute]int? gameId)
		{
			var skill = _evaluationDbController.GetByGame(gameId);
			var skillContract = skill.ToContractList();
			return new ObjectResult(skillContract);
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
		public IActionResult GetGameProgress([FromRoute]int gameId, [FromRoute]int? actorId)
		{
			var skills = _evaluationDbController.GetByGame(gameId);
			skills = _evaluationController.FilterByActorType(skills, actorId);
			var skillResponses = skills.Select(a =>
			{
				var completed = _evaluationController.IsEvaluationCompleted(a, actorId);
				return new EvaluationProgressResponse
				{
					Name = a.Name,
					Progress = completed,
				};
			});

			return new ObjectResult(skillResponses);
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
		public IActionResult GetAchievementProgress([FromRoute]string token, [FromRoute]int? gameId, [FromRoute]int? actorId)
		{
			var skill = _evaluationDbController.Get(token, gameId);
			var completed = _evaluationController.IsEvaluationCompleted(skill, actorId);
			return new ObjectResult(new EvaluationProgressResponse
			{
				Name = skill.Name,
				Progress = completed,
			});
		}

		/// <summary>
		/// Create a new Skill.
		/// Requires <see cref="EvaluationRequest.Name"/> to be unique to that <see cref="EvaluationRequest.GameId"/>.
		/// 
		/// Example Usage: POST api/skills/create
		/// </summary>
		/// <param name="newSkill"><see cref="EvaluationRequest"/> object that holds the details of the new Skill.</param>
		/// <returns>Returns a <see cref="EvaluationResponse"/> object containing details for the newly created Skill.</returns>
		[HttpPost("create")]
		//[ResponseType(typeof(EvaluationResponse))]
		[ArgumentsNotNull]
		public IActionResult Create([FromBody] EvaluationCreateRequest newSkill)
		{
			var skill = newSkill.ToSkillModel();
			_evaluationDbController.Create(skill);
			var skillContract = skill.ToContract();
			return new ObjectResult(skillContract);
		}

		/// <summary>
		/// Update an existing Skill.
		/// 
		/// Example Usage: PUT api/skills/update
		/// </summary>
		/// <param name="skill"><see cref="EvaluationRequest"/> object that holds the details of the Skill.</param>
		[HttpPut("update")]
		[ArgumentsNotNull]
		public void Update([FromBody] EvaluationUpdateRequest skill)
		{
			var skillModel = skill.ToSkillModel();
			_evaluationDbController.Update(skillModel);
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
		public void Delete([FromRoute]string token, [FromRoute]int? gameId)
		{
			_evaluationDbController.Delete(token, gameId);
		}
	}
}