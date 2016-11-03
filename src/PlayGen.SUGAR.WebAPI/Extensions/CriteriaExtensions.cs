using System.Collections.Generic;
using System.Linq;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class CriteriaExtensions
    {
		public static Contracts.Shared.CompletionCriteria ToContract(this Common.Shared.CompletionCriteria completionCriteria)
		{
			if (completionCriteria == null)
			{
				return null;
			}
			return new Contracts.Shared.CompletionCriteria
			{
				Key = completionCriteria.Key,
				DataType = completionCriteria.DataType,
				CriteriaQueryType = completionCriteria.CriteriaQueryType,
				ComparisonType = completionCriteria.ComparisonType,
				Scope = completionCriteria.Scope,
				Value = completionCriteria.Value,
			};
		}

		public static List<Contracts.Shared.CompletionCriteria> ToContractList(this List<Common.Shared.CompletionCriteria> completionCriterias)
		{
			return completionCriterias.Select(ToContract).ToList();
		}

        public static Common.Shared.CompletionCriteria ToModel(this Contracts.Shared.CompletionCriteria completionCriteria)
        {
            if (completionCriteria == null)
            {
                return null;
            }
            return new Common.Shared.CompletionCriteria
            {
                Key = completionCriteria.Key,
                DataType = completionCriteria.DataType,
                CriteriaQueryType = completionCriteria.CriteriaQueryType,
                ComparisonType = completionCriteria.ComparisonType,
                Scope = completionCriteria.Scope,
                Value = completionCriteria.Value,
            };
        }

        public static List<Common.Shared.CompletionCriteria> ToModelList(this List<Contracts.Shared.CompletionCriteria> completionCriterias)
        {
            return completionCriterias.Select(ToModel).ToList();
        }
    }
}
