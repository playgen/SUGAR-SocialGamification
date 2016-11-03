using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	internal static class AchievementExtensions
	{
        public static AchievementResponse ToContract(this Achievement model)
		{
			if (model == null)
			{
				return null;
			}

            return new AchievementResponse
			{
				Name = model.Name,
				Description = model.Description,
				GameId = model.GameId,
				ActorType = model.ActorType,
				Token = model.Token,
				CompletionCriterias = model.CompletionCriterias.ToContractList(),
				Rewards = model.Rewards.ToContractList(),
			};
		}

        public static IEnumerable<AchievementResponse> ToContractList(this IEnumerable<Achievement> models)
		{
			return models.Select(ToContract).ToList();
		}

        public static Achievement ToAchievementModel(this AchievementRequest contract)
		{
			return new Achievement
			{
				Name = contract.Name,
				Description = contract.Description,
				GameId = contract.GameId ?? 0,
				ActorType = contract.ActorType,
				Token = contract.Token,
				CompletionCriterias = contract.CompletionCriterias.ToModelList(),
				Rewards = contract.Rewards.ToModelList(),
			};
		}

        public static Common.Shared.CompletionCriteria ToModel(this Contracts.Shared.CompletionCriteria contract)
		{
			return new Common.Shared.CompletionCriteria
            {
				Key = contract.Key,
				ComparisonType = contract.ComparisonType,
				CriteriaQueryType = contract.CriteriaQueryType,
				DataType = contract.DataType,
				Scope = contract.Scope,
				Value = contract.Value,
			};
		}
        
		public static Common.Shared.Reward ToModel(this Contracts.Shared.Reward contract)
		{
			return new Common.Shared.Reward
			{
				Key = contract.Key,
				DataType = contract.DataType,
				Value = contract.Value,
			};
		}
	}
}