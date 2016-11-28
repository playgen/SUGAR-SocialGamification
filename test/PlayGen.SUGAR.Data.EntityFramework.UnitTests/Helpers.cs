using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
    public static class Helpers
    {
        public static User CreateUser(string name)
        {
            return ControllerLocator.UserController.Create(new User
            {
                Name = name
            });
        }

        public static Game CreateGame(string name)
        {
            return ControllerLocator.GameController.Create(new Game
            {
                Name = name
            });
        }

        public static Achievement CreateAchievement(string name, int? gameId = null, bool addCriteria = true)
        {
            if (gameId == null)
            {
                var game = new Game
                {
                    Name = name
                };
                ControllerLocator.GameController.Create(game);
                gameId = game.Id;
            }

            var achievement = new Achievement
            {
                Name = name,
                Token = name,
                GameId = gameId.Value,
                ActorType = ActorType.User,
                EvaluationCriterias = new List<Model.EvaluationCriteria>(),
                Rewards = new List<Model.Reward>()
            };
            if (addCriteria)
            {
                var criteria = new List<Model.EvaluationCriteria>
                {
                    new Model.EvaluationCriteria
                    {
                        Key = "CreateAchievementKey",
                        DataType = SaveDataType.String,
                        CriteriaQueryType = CriteriaQueryType.Any,
                        ComparisonType = ComparisonType.Equals,
                        Scope = CriteriaScope.Actor,
                        Value = "CreateAchievementValue"
                    }
                };
                achievement.EvaluationCriterias = criteria;
            }

            ControllerLocator.EvaluationController.Create(achievement);

            return achievement;
        }
    }
}
