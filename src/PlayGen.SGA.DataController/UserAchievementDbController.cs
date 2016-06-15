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

                var achievement = new UserAchievement
                {
                    Name = newAchievement.Name,
                    GameId = newAchievement.GameId,
                    CompletionCriteria = newAchievement.CompletionCriteria
                };
                context.UserAchievements.Add(achievement);
                context.SaveChanges();

                return achievement;
            }
        }

        public Game Get(string name)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var game = context.Games.Single(g => g.Name == name);

                return game;
            }
        }

        public void Delete(int id)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var game = context.Games.Single(g => g.Id == id);

                context.Games.Remove(game);
                context.SaveChanges();
            }
        }
    }
}