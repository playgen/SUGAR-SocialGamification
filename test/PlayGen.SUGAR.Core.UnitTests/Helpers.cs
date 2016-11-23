using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using EvaluationCriteria = PlayGen.SUGAR.Data.Model.EvaluationCriteria;
using Reward = PlayGen.SUGAR.Data.Model.Reward;

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

		public static Evaluation ComposeEvaluation(string name, int? gameId = null)
		{
			var evaluationCriterias = new List<EvaluationCriteria>();
			for (var i = 0; i < 2; i++)
			{
				evaluationCriterias.Add(new EvaluationCriteria
				{
					Id = 1,
					Key = $"{name}_{i}",
					DataType = GameDataType.Long,
					CriteriaQueryType = CriteriaQueryType.Sum,
					ComparisonType = ComparisonType.GreaterOrEqual,
					Scope = CriteriaScope.Actor,
					Value = "100"
				});
			}

			return new Data.Model.Achievement
			{
				// Arrange
				Id = 1,
				Token = name,

				Name = name,
				Description = name,

				ActorType = ActorType.User,
				GameId = gameId,

				EvaluationCriterias = evaluationCriterias
			};
		}

		public static GameData ComposeGameData(EvaluationCriteria evaluationCriteria, int? gameId = null)
		{
			return new GameData
			{
				Key = evaluationCriteria.Key,
				DataType = evaluationCriteria.DataType,

				GameId = gameId,
				Value = "50"
			};
		}

		public static Evaluation CreateGenericAchievement(string key, int? gameId = null)
		{
			return ControllerLocator.EvaluationController.Create(new Achievement
			{
				GameId = gameId,
				Name = key,
				Description = key,
				ActorType = ActorType.User,
				Token = key,
				EvaluationCriterias = new List<EvaluationCriteria>
				{
					new EvaluationCriteria
					{
						ComparisonType = ComparisonType.GreaterOrEqual,
						CriteriaQueryType = CriteriaQueryType.Sum,
						DataType = GameDataType.Long,
						Key = key,
						Scope = CriteriaScope.Actor,
						Value = $"{100}",
					}
				},
				Rewards = null,
			});
		}

		public static void CompleteGenericAchievement(Evaluation evaluation, int userId)
		{
			ControllerLocator.GameDataController.Add(new GameData
			{
				ActorId = userId,
				DataType = evaluation.EvaluationCriterias[0].DataType,
				Value = $"{200}",
				GameId = evaluation.GameId,
				Key = evaluation.EvaluationCriterias[0].Key
			});
		}
	}
}