using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class CriteriaExtensions
    {
		public static AchievementCriteria ToContract(this AchievementCriteria completionCriteria)
		{
			return new AchievementCriteria
			{
				Key = completionCriteria.Key,
				DataType = completionCriteria.DataType,
				CriteriaQueryType = completionCriteria.CriteriaQueryType,
				ComparisonType = completionCriteria.ComparisonType,
				Scope = completionCriteria.Scope,
				Value = completionCriteria.Value,
			};
		}

		public static List<Contracts.AchievementCriteria> ToContractList(this AchievementCriteriaCollection completionCriteriaCollection)
		{
			return completionCriteriaCollection.Select(completionCriteria => completionCriteria.ToContract()).ToList();
		}
	}
}
