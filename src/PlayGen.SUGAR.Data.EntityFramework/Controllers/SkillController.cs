using System.Linq;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class SkillController : OLD_DbController
	{
		public SkillController(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		public IEnumerable<Skill> GetByGame(int? gameId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);
				gameId = gameId ?? 0;

				var skills = context.Skills.Where(a => a.GameId == gameId).ToList();
				return skills;
			}
		}

		public Skill Get(string token, int? gameId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				gameId = gameId ?? 0;

				var skill = context.Skills.Find(token, gameId);
				return skill;
			}
		}

		public Skill Create(Skill skill)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				//TODO: refine duplicate text for actor type and game id
				var hasConflicts = context.Skills.Any(a => (a.Name == skill.Name && a.GameId == skill.GameId)
									|| (a.Token == skill.Token && a.GameId == skill.GameId));

				if (hasConflicts)
				{
					throw new DuplicateRecordException($"An skill with the name {skill.Name} or token {skill.Token} for this game already exists.");
				}

				context.Skills.Add(skill);
				SaveChanges(context);
				return skill;
			}
		}

		public void Update(Skill skill)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var existing = context.Skills.Find(skill.Token, skill.GameId);

				if (existing != null)
				{
					context.Entry(existing).State = EntityState.Modified;

					existing.Name = skill.Name;
					existing.CompletionCriteriaCollection = skill.CompletionCriteriaCollection;
					existing.RewardCollection = skill.RewardCollection;
					existing.Description = skill.Description;
					existing.ActorType = skill.ActorType;
					existing.GameId = skill.GameId;
					existing.Token = skill.Token;

					SaveChanges(context);
				}
				else
				{
					throw new MissingRecordException($"The existing skill with token {skill.Token} and game ID {skill.GameId} could not be found.");
				}
			}
		}

		public void Delete(string token, int? gameId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				gameId = gameId ?? 0;

				var skill = context.Skills.Find(token, gameId);
				if (skill != null)
				{
					context.Skills.Remove(skill);
					SaveChanges(context);
				}
			}
		}
	}
}
