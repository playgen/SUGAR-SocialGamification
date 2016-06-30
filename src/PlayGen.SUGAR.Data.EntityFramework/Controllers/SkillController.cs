using System.Linq;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class SkillController : DbController
	{
		public SkillController(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		public IEnumerable<Skill> GetGlobal()
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var skills = context.Skills.Where(a => a.GameId == null).ToList();
				return skills;
			}
		}


		public IEnumerable<Skill> GetByGame(int gameId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var skills = context.Skills.Where(a => a.GameId == gameId).ToList();
				return skills;
			}
		}

		public Skill Get(int skillId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var skill = context.Skills.Find(skillId);
				return skill;
			}
		}

		public Skill Create(Skill skill)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				//TODO: refine duplicate text for actor type and game id
				var hasConflicts = context.Skills.Any(a => a.Name == skill.Name && a.GameId == skill.GameId);

				if (hasConflicts)
				{
					throw new DuplicateRecordException($"An skill with the name {skill.Name} for this game already exists.");
				}

				context.Skills.Add(skill);
				SaveChanges(context);
				return skill;
			}
		}

		public void Delete(int id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var skill = context.Skills.Find(id);
				if (skill != null)
				{
					context.Skills.Remove(skill);
					SaveChanges(context);
				}
			}
		}
	}
}
