using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Core.Controllers;
using EvaluationCriteria = PlayGen.SUGAR.Data.Model.EvaluationCriteria;
using DbControllerLocator = PlayGen.SUGAR.Data.EntityFramework.UnitTests.ControllerLocator;

namespace PlayGen.SUGAR.Core.UnitTests
{
	public static class Helpers
	{
		public static User GetOrCreateUser(string name)
		{
			User user;
			var users = ControllerLocator.UserController.Search(name, true);

			if (users.Any())
			{
				user = users.ElementAt(0);
			}
			else
			{
				user = ControllerLocator.UserController.Create(new User
				{
					Name = name,
				});
			}

			return user;
		}

		public static Game GetOrCreateGame(string name)
		{
			Game game;
			var games = ControllerLocator.GameController.Search(name);

			if (games.Any())
			{
				game = games.ElementAt(0);
			}
			else
			{
				game = ControllerLocator.GameController.Create(new Game
				{
					Name = name,
				}, 1);
			}

			return game;
		}

		public static Evaluation ComposeGenericAchievement(string key, int? gameId = null, int evaluationCriteriaCount = 1)
		{
			var evaluationCriterias = new List<EvaluationCriteria>();
			for (var i = 0; i < evaluationCriteriaCount; i++)
			{
				evaluationCriterias.Add(new EvaluationCriteria
				{
					EvaluationDataKey = $"{key}_{i}",
					EvaluationDataType = EvaluationDataType.Long,
					CriteriaQueryType = CriteriaQueryType.Sum,
					ComparisonType = ComparisonType.GreaterOrEqual,
					Scope = CriteriaScope.Actor,
					Value = "100"
				});
			}

			return new Data.Model.Achievement
			{
				// Arrange
				Token = key,

				Name = key,
				Description = key,

				ActorType = ActorType.User,
				GameId = gameId,

				EvaluationCriterias = evaluationCriterias
			};
		}

        public static List<EvaluationData> ComposeAchievementGameDatas(int actorId, Evaluation evaluation, string value = "50")
        {
            var gameDatas = new List<EvaluationData>();

            foreach (var criteria in evaluation.EvaluationCriterias)
            {
                gameDatas.Add(ComposeEvaluationData(actorId, criteria, evaluation.GameId, value));
            }

            return gameDatas;
        }

        public static EvaluationData ComposeEvaluationData(int actorId, EvaluationCriteria evaluationCriteria, int? gameId = null, string value = "50")
		{
			return new EvaluationData
			{
                Key = evaluationCriteria.EvaluationDataKey,
                EvaluationDataType = evaluationCriteria.EvaluationDataType,

                ActorId = actorId,
				GameId = gameId,

                Value = value
			};
		}

		public static Evaluation CreateGenericAchievement(string key, int? gameId = null)
		{
		    return ControllerLocator.EvaluationController.Create(ComposeGenericAchievement(key, gameId));
		}

        public static void CreateGenericAchievementGameData(Evaluation evaluation, int actorId, string value = "50")
        {
            var gameDatas = ComposeAchievementGameDatas(actorId, evaluation, value);
            
            var evaluationDataController = new EvaluationDataController(DbControllerLocator.ContextFactory, evaluation.EvaluationCriterias[0].EvaluationDataCategory);
            evaluationDataController.Add(gameDatas.ToArray());
        }

        public static void CompleteGenericAchievement(Evaluation evaluation, int actorId)
		{
		    var gameDatas = ComposeAchievementGameDatas(actorId, evaluation, "100");

            var evaluationDataController = new EvaluationDataController(DbControllerLocator.ContextFactory, evaluation.EvaluationCriterias[0].EvaluationDataCategory);
            evaluationDataController.Add(gameDatas.ToArray());
        }

	    public static Evaluation CreateAndCompleteGenericAchievement(string key, int actorId, int? gameId = null)
	    {
	        var evaluation = CreateGenericAchievement(key, gameId);
	        CompleteGenericAchievement(evaluation, actorId);

	        return evaluation;
	    }
	}
}