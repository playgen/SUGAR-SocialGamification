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
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var achievements = context.UserAchievements.Where(a => gameIds.Contains(a.GameId)).ToList();

                return achievements;
            }
        }

        public UserAchievement Create(UserAchievement newAchievement)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.UserAchievements.Any(a => a.Name == newAchievement.Name && a.GameId == newAchievement.GameId);

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("An achievement with the name {0} for this game already exists.", newAchievement.Name));
                }

                var gameExists = context.Games.Any(g => g.Id == newAchievement.GameId);

                if (!gameExists)
                {
                    throw new DuplicateRecordException(string.Format("The provided game does not exist."));
                }

                var achievement = newAchievement;
                context.UserAchievements.Add(achievement);
                context.SaveChanges();

                return achievement;
            }
        }

        public void Delete(int[] id)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var achievement = context.UserAchievements.Where(u => id.Contains(u.Id)).ToList();

                context.UserAchievements.RemoveRange(achievement);
                context.SaveChanges();
            }
        }
    }
}