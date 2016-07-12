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
	/// Web Controller that facilitates Skill specific operations.
	/// </summary>
	[Route("api/[controller]")]
		[Authorization]
		public class SkillsController : Controller
	{
		private readonly Data.EntityFramework.Controllers.SkillController _skillController;
		private readonly SkillController _skillEvaluationController;

		public SkillsController(Data.EntityFramework.Controllers.SkillController skillController,
			SkillController skillEvaluationController)
		{
			_skillController = skillController;
			_skillEvaluationController = skillEvaluationController;
		}

		/// <summary>
		/// Find a Skill that matches <param name="token"/> and <param name="gameId"/>.
		/// 
		/// Example Usage: GET api/skills/find/SKILL_TOKEN/1
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		/// <returns>Returns <see cref="AchievementResponse"/> that holds Skill details</returns>
		[HttpGet("find/{token}/{gameId:int}")]
		[HttpGet("find/{token}/global")]
		[ResponseType(typeof(AchievementResponse))]
		public IActionResult Get([FromRoute]string token, [FromRoute]int? gameId)
		{
			var skill = _skillController.Get(token, gameId);
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
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Skill details</returns>
		[HttpGet("global/list")]
		[HttpGet("game/{gameId:int}/list")]
		[ResponseType(typeof(IEnumerable<AchievementResponse>))]
		public IActionResult Get([FromRoute]int? gameId)
		{
			var skill = _skillController.GetByGame(gameId);
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
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current progress toward skill.</returns>
		[HttpGet("game/{gameId:int}/evaluate")]
		[HttpGet("global/evaluate")]
		[HttpGet("game/{gameId:int}/evaluate/{actorId:int}")]
		[HttpGet("global/evaluate/{actorId:int}")]
		[ResponseType(typeof(IEnumerable<AchievementProgressResponse>))]
		public IActionResult GetGameProgress([FromRoute]int gameId, [FromRoute]int? actorId)
		{
			var skills = _skillController.GetByGame(gameId);
			skills = _skillEvaluationController.FilterByActorType(skills, actorId);
			var skillResponses = skills.Select(a =>
			{
				var completed = _skillEvaluationController.IsSkillCompleted(a, actorId);
				return new AchievementProgressResponse
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
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current progress toward skill.</returns>
		[HttpGet("{token}/{gameId:int}/evaluate")]
		[HttpGet("{token}/global/evaluate")]
		[HttpGet("{token}/{gameId:int}/evaluate/{actorId:int}")]
		[HttpGet("{token}/global/evaluate/{actorId:int}")]
		[ResponseType(typeof(AchievementProgressResponse))]
		public IActionResult GetAchievementProgress([FromRoute]string token, [FromRoute]int? gameId, [FromRoute]int? actorId)
		{
			var skill = _skillController.Get(token, gameId);
			var completed = _skillEvaluationController.IsSkillCompleted(skill, actorId);
			return new ObjectResult(new AchievementProgressResponse
			{
				Name = skill.Name,
				Progress = completed,
			});
		}

		/// <summary>
		/// Create a new Skill.
		/// Requires <see cref="AchievementRequest.Name"/> to be unique to that <see cref="AchievementRequest.GameId"/>.
		/// 
		/// Example Usage: POST api/skills/create
		/// </summary>
		/// <param name="newSkill"><see cref="AchievementRequest"/> object that holds the details of the new Skill.</param>
		/// <returns>Returns a <see cref="AchievementResponse"/> object containing details for the newly created Skill.</returns>
		[HttpPost("create")]
		[ResponseType(typeof(AchievementResponse))]
		[ArgumentsNotNull]
		public IActionResult Create([FromBody] AchievementRequest newSkill)
		{
			var skill = newSkill.ToSkillModel();
			_skillController.Create(skill);
			var skillContract = skill.ToContract();
			return new ObjectResult(skillContract);
		}

		/// <summary>
		/// Update an existing Skill.
		/// 
		/// Example Usage: PUT api/skills/update
		/// </summary>
		/// <param name="skill"><see cref="AchievementRequest"/> object that holds the details of the Skill.</param>
		[HttpPut("update")]
		[ArgumentsNotNull]
		public void Update([FromBody] AchievementRequest skill)
		{
			var skillModel = skill.ToSkillModel();
			_skillController.Update(skillModel);
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
			_skillController.Delete(token, gameId);
		}
	}
}