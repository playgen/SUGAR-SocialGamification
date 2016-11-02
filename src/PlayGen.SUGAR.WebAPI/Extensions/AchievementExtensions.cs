using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	internal static class AchievementExtensions
	{
		internal static AchievementResponse ToContract(this Achievement model)
		{
			if (model == null)
			{
				return null;
			}
			var achievementContract = new AchievementResponse
			{
				Name = model.Name,
				Description = model.Description,
				GameId = model.GameId,
				ActorType = model.ActorType,
				Token = model.Token,
				CompletionCriterias = model.CompletionCriterias.ToContract(),
				Rewards = model.Rewards.ToContract(),
			};

			return achievementContract;
		}

		internal static IEnumerable<AchievementResponse> ToContract(this IEnumerable<Achievement> models)
		{
			return models.Select(ToContract).ToList();
		}

		internal static Achievement ToModel(this AchievementRequest contract)
		{
			var achieveModel = new Achievement
			{
				Name = contract.Name,
				Description = contract.Description,
				GameId = contract.GameId ?? 0,
				ActorType = contract.ActorType,
				Token = contract.Token,
				CompletionCriterias = contract.CompletionCriterias.ToModel(),
				Rewards = contract.Rewards.ToModel(),
			};

			return achieveModel;
		}

		internal static List<AchievementCriteria> ToModel(this List<Contracts.AchievementCriteria> contracts)
		{
			var models = new List<Data.Model.Achievement>();
			if (contracts != null)
			{
                models.Add(contracts.Select(ToModel).ToList());
			}

			return models;
		}

		internal static AchievementCriteria ToModel(this AchievementCriteria contract)
		{
			return new AchievementCriteria
			{
				Key = contract.Key,
				ComparisonType = contract.ComparisonType,
				CriteriaQueryType = contract.CriteriaQueryType,
				DataType = contract.DataType,
				Scope = contract.Scope,
				Value = contract.Value,
			};
		}
        
		internal static Reward ToModel(this Reward contract)
		{
			return new Reward
			{
				Key = contract.Key,
				DataType = contract.DataType,
				Value = contract.Value,
			};
		}
	}
}