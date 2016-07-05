using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class AchievementExtensions
	{
		public static AchievementResponse ToContract(this Achievement model)
		{
			var achievementContract = new AchievementResponse
			{
				Id = model.Id,
				Name = model.Name,
				Description = model.Description,
				GameId = model.GameId,
				ActorType = model.ActorType,
				Token = model.Token,
				CompletionCriteria = model.CompletionCriteriaCollection.ToContractList(),
				Reward = model.RewardCollection.ToContractList(),
			};

			return achievementContract;
		}

		public static IEnumerable<AchievementResponse> ToContractList(this IEnumerable<Achievement> models)
		{
			return models.Select(ToContract).ToList();
		}

		public static Achievement ToAchievementModel(this AchievementRequest achieveContract)
		{
			var achieveModel = new Achievement
			{
				Name = achieveContract.Name,
				Description = achieveContract.Description,
				GameId = achieveContract.GameId,
				ActorType = achieveContract.ActorType,
				Token = achieveContract.Token,
				CompletionCriteriaCollection = achieveContract.CompletionCriteria.ToAchievementModel(),
				RewardCollection = achieveContract.Reward.ToAchievementModel(),
			};

			return achieveModel;
		}

		public static AchievementCriteriaCollection ToAchievementModel(this List<AchievementCriteria> achievementContracts)
		{
			var achievementCollection = new AchievementCriteriaCollection();
			if (achievementContracts != null)
			{
				achievementCollection.Add(achievementContracts.Select(ToModel).ToList());
			}

			return achievementCollection;
		}

		public static AchievementCriteria ToModel(this Contracts.AchievementCriteria achievementContract)
		{
			return new AchievementCriteria
			{
				Key = achievementContract.Key,
				ComparisonType = (ComparisonType)achievementContract.ComparisonType,
				DataType = (GameDataType)achievementContract.DataType,
				Scope = (CriteriaScope)achievementContract.Scope,
				Value = achievementContract.Value,
			};
		}

		public static RewardCollection ToAchievementModel(this List<Reward> achievementContracts)
		{
			var rewardCollection = new RewardCollection();
			if (achievementContracts != null)
			{
				rewardCollection.Add(achievementContracts.Select(ToModel).ToList());
			}

			return rewardCollection;
		}

		public static Reward ToModel(this Contracts.Reward achievementContract)
		{
			return new Reward
			{
				Key = achievementContract.Key,
				DataType = (GameDataType)achievementContract.DataType,
				Value = achievementContract.Value,
			};
		}
	}
}