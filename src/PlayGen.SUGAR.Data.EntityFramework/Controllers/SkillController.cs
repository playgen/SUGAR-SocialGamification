using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class SkillController : DbController
	{
		public SkillController(SUGARContextFactory contextFactory) 
			: base(contextFactory)
		{
		}

		public IEnumerable<Skill> GetByGame(int? gameId)
		{
			using (var context = ContextFactory.Create())
			{
				gameId = gameId ?? 0;

				var skills = context.Skills
					.IncludeAll()
					.Where(a => a.GameId == gameId).ToList();
				return skills;
			}
		}

		public Skill Get(string token, int? gameId)
		{
			using (var context = ContextFactory.Create())
			{
				gameId = gameId ?? 0;

				var skill = context.Skills
					.IncludeAll()
					.Find(context, token, gameId);
				return skill;
			}
		}

		public Skill Create(Skill skill)
		{
			using (var context = ContextFactory.Create())
			{
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
			using (var context = ContextFactory.Create())
			{
				var existing = context.Skills
					.IncludeAll()
					.Find(context, skill.Token, skill.GameId);

				if (existing != null)
				{
					context.Entry(existing).State = EntityState.Modified;

					var hasConflicts = context.Skills
						.Where(a => (a.Name == skill.Name && a.GameId == skill.GameId));

					if (hasConflicts.Count() > 0)
					{
						if (hasConflicts.Any(a => a.Token != skill.Token))
						{
							throw new DuplicateRecordException($"A skill with the name {skill.Name} for this game already exists.");
						}
					}

					existing.Name = skill.Name;
					existing.CompletionCriterias = skill.CompletionCriterias;
					existing.Rewards = skill.Rewards;
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
			using (var context = ContextFactory.Create())
			{
				gameId = gameId ?? 0;

				var skill = context.Skills
					.IncludeAll()
					.Find(context, token, gameId);

				if (skill != null)
				{
					context.Skills.Remove(skill);
					SaveChanges(context);
				}
			}
		}
	}
}
