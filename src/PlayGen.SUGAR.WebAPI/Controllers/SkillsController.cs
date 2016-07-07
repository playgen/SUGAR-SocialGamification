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
		/// Get all global skills, ie. skills that are not associated with a specific game
		/// 
		/// Example Usage: GET api/skills/list
		/// </summary>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Skill details</returns>
		[HttpGet("list")]
		[ResponseType(typeof(IEnumerable<AchievementResponse>))]
		public IActionResult Get()
		{
			var skill = _skillController.GetGlobal();
			var skillContract = skill.ToContractList();
			return new ObjectResult(skillContract);
		}

		/// <summary>
		/// Find a list of Skills that match <param name="gameId"/>.
		/// 
		/// Example Usage: GET api/skills/game/1/list
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Skill details</returns>
		[HttpGet("game/{gameId:int}/list")]
		[ResponseType(typeof(IEnumerable<AchievementResponse>))]
		public IActionResult Get([FromRoute]int gameId)
		{
			var skill = _skillController.GetByGame(gameId);
			var skillContract = skill.ToContractList();
			return new ObjectResult(skillContract);
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
		/// Example Usage: PUT api/skills/update/1
		/// </summary>
		/// <param name="id">Id of the existing Skill.</param>
		/// <param name="skill"><see cref="AchievementRequest"/> object that holds the details of the Skill.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
		public void Update([FromRoute] int id, [FromBody] AchievementRequest skill)
		{
			var skillModel = skill.ToSkillModel();
			skillModel.Id = id;
			_skillController.Update(skillModel);
		}

		/// <summary>
		/// Delete Skills with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/skills/1
		/// </summary>
		/// <param name="id">Skill ID</param>
		[HttpDelete("{id:int}")]
		public void Delete([FromRoute]int id)
		{
			_skillController.Delete(id);
		}

		/// <summary>
		/// Find the current progress for all skills for a <param name="gameId"/> for <param name="actorId"/>.
		/// 
		/// Example Usage: GET api/skills/game/1/evaluate/1
		/// </summary>
		/// <param name="actorId">ID of Group/User</param>
		/// <param name="gameId">ID of Game</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current group progress toward skill.</returns>
		[HttpGet("game/{gameId:int}/evaluate")]
		[HttpGet("game/{gameId:int}/evaluate/{actorId:int}")]
		[ResponseType(typeof(IEnumerable<AchievementProgressResponse>))]
		public IActionResult GetGameProgress([FromRoute]int gameId, [FromRoute]int? actorId)
		{
			var skills = _skillController.GetByGame(gameId);
			var skillResponses = skills.Select(a =>
			{
				var completed = _skillEvaluationController.IsSkillCompleted(a, actorId);
				return new AchievementProgressResponse
				{
					Name = a.Name,
					Progress = completed ? 1 : 0,
				};
			});

			return new ObjectResult(skillResponses);
		}

		/// <summary>
		/// Find the current progress for an <param name="skillId"/> for <param name="actor"/>.
		/// 
		/// Example Usage: GET api/skills/1/evaluate/1
		/// </summary>
		/// <param name="skillId">ID of Skill</param>
		/// <param name="actor">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current group progress toward skill.</returns>
		[HttpGet("{skillId:int}/evaluate")]
		[HttpGet("{skillId:int}/evaluate/{actorId:int}")]
		[ResponseType(typeof(AchievementProgressResponse))]
		public IActionResult GetAchievementProgress([FromRoute]int skillId, [FromRoute]int? actorId)
		{
			var skill = _skillController.Get(skillId);
			var completed = _skillEvaluationController.IsSkillCompleted(skill, actorId);
			return new ObjectResult(new AchievementProgressResponse
			{
				Name = skill.Name,
				Progress = completed ? 1 : 0,
			});
		}
	}
}