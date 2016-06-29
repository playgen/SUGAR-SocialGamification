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
				GameId = model.GameId,
				ActorType = model.ActorType,
				CompletionCriteria = model.CompletionCriteriaCollection.ToContractList(),
				Reward = model.RewardCollection.ToContractList(),
			};

			return achievementContract;
		}

		public static IEnumerable<AchievementResponse> ToContractList(this IEnumerable<Achievement> models)
		{
			return models.Select(ToContract).ToList();
		}

		public static AchievementCriteria ToContract(this AchievementCriteria completionCriteria)
		{
			return new AchievementCriteria
			{
				Key = completionCriteria.Key,
				DataType = completionCriteria.DataType,
				ComparisonType = completionCriteria.ComparisonType,
				Scope = completionCriteria.Scope,
				Value = completionCriteria.Value,
			};
		}

		public static List<Contracts.AchievementCriteria> ToContractList(this AchievementCriteriaCollection completionCriteriaCollection)
		{
			return completionCriteriaCollection.Select(completionCriteria => completionCriteria.ToContract()).ToList();
		}

		public static Reward ToContract(this Reward reward)
		{
			return new Reward
			{
				Key = reward.Key,
				DataType = reward.DataType,
				Value = reward.Value,
			};
		}

		public static List<Contracts.Reward> ToContractList(this RewardCollection rewardCollection)
		{
			return rewardCollection.Select(reward => reward.ToContract()).ToList();
		}

		public static Achievement ToModel(this AchievementRequest achieveContract)
		{
			var achieveModel = new Achievement
			{
				Name = achieveContract.Name,
				GameId = achieveContract.GameId,
				ActorType = achieveContract.ActorType,
				CompletionCriteriaCollection = achieveContract.CompletionCriteria.ToModel(),
				RewardCollection = achieveContract.Reward.ToModel(),
			};

			return achieveModel;
		}

		public static AchievementCriteriaCollection ToModel(this List<AchievementCriteria> achievementContracts)
		{
			var achievementCollection = new AchievementCriteriaCollection();
			foreach (var achievementContract in achievementContracts)
			{
				achievementCollection.Add(achievementContract.ToModel());
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

		public static RewardCollection ToModel(this List<Reward> achievementContracts)
		{
			var rewardCollection = new RewardCollection();
			foreach (var achievementContract in achievementContracts)
			{
				rewardCollection.Add(achievementContract.ToModel());
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