using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.DataController
{
    public class UserAchievementDbController : DbController
    {
        public UserAchievementDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public IEnumerable<UserAchievement> Get(int[] gameIds)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var achievements = context.UserAchievements.Where(a => gameIds.Contains(a.GameId)).ToList();

                return achievements;
            }
        }

        public UserAchievement Create(UserAchievement achievement)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.UserAchievements.Any(a => a.Name == achievement.Name && a.GameId == achievement.GameId);

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("An achievement with the name {0} for this game already exists.", achievement.Name));
                }

                var gameExists = context.Games.Any(g => g.Id == achievement.GameId);

                if (!gameExists)
                {
                    throw new DuplicateRecordException(string.Format("The provided game does not exist."));
                }

                context.UserAchievements.Add(achievement);
                SaveChanges(context);
                return achievement;
            }
        }

        public void Delete(int[] id)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var achievement = context.UserAchievements.Where(u => id.Contains(u.Id)).ToList();

                context.UserAchievements.RemoveRange(achievement);
                SaveChanges(context);
            }
        }
    }
}