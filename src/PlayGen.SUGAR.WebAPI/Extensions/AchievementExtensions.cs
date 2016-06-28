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
				CompletionCriteria = model.CompletionCriteriaCollection.ToContractList()
			};

			return achievementContract;
		}

		public static IEnumerable<AchievementResponse> ToContractList(this IEnumerable<Achievement> groupModels)
		{
			return groupModels.Select(ToContract).ToList();
		}

		public static AchievementCriteria ToContract(this AchievementCriteria completionCriteria)
		{
			return new AchievementCriteria
			{
				Key = completionCriteria.Key,
				DataType = completionCriteria.DataType,
				ComparisonType = completionCriteria.ComparisonType,
				Value = completionCriteria.Value,
			};
		}

		public static List<Contracts.AchievementCriteria> ToContractList(this AchievementCriteriaCollection completionCriteriaCollection)
		{
			return completionCriteriaCollection.Select(completionCriteria => completionCriteria.ToContract()).ToList();
		}

		public static Achievement ToModel(this AchievementRequest achieveContract)
		{
			var achieveModel = new Achievement
			{
				Name = achieveContract.Name,
				GameId = achieveContract.GameId,
				CompletionCriteriaCollection = achieveContract.CompletionCriteria.ToModel()
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
				Value = achievementContract.Value,
			};
		}
	}
}