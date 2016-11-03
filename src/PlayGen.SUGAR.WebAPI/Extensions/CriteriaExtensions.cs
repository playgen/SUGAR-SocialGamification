using System.Collections.Generic;
using System.Linq;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class CriteriaExtensions
    {
		public static Contracts.Shared.CompletionCriteria ToContract(this Data.Model.CompletionCriteria completionCriteria)
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

		public static List<Contracts.Shared.CompletionCriteria> ToContractList(this List<Data.Model.CompletionCriteria> completionCriterias)
		{
			return completionCriterias.Select(ToContract).ToList();
		}

        public static Data.Model.CompletionCriteria ToModel(this Contracts.Shared.CompletionCriteria completionCriteria)
        {
            if (completionCriteria == null)
            {
                return null;
            }
            return new Data.Model.CompletionCriteria
            {
                Key = completionCriteria.Key,
                DataType = completionCriteria.DataType,
                CriteriaQueryType = completionCriteria.CriteriaQueryType,
                ComparisonType = completionCriteria.ComparisonType,
                Scope = completionCriteria.Scope,
                Value = completionCriteria.Value,
            };
        }

        public static List<Data.Model.CompletionCriteria> ToModelList(this List<Contracts.Shared.CompletionCriteria> completionCriterias)
        {
            return completionCriterias.Select(ToModel).ToList();
        }
    }
}
