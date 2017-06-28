using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class EvaluationCriteriaExtensions
	{
		public static EvaluationCriteriaResponse ToContract(this EvaluationCriteria completionCriteria)
		{
			if (completionCriteria == null)
				return null;
			return new EvaluationCriteriaResponse
			{
				Id = completionCriteria.Id,
				EvaluationDataKey = completionCriteria.EvaluationDataKey,
				EvaluationDataType = completionCriteria.EvaluationDataType,
				CriteriaQueryType = completionCriteria.CriteriaQueryType,
				ComparisonType = completionCriteria.ComparisonType,
				Scope = completionCriteria.Scope,
				Value = completionCriteria.Value
			};
		}

		public static EvaluationCriteriasResponse ToCollectionContract(this List<EvaluationCriteria> models)
		{
			return new EvaluationCriteriasResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}

		public static EvaluationCriteria ToModel(this EvaluationCriteriaCreateRequest completionCriteria)
		{
			if (completionCriteria == null)
				return null;
			return new EvaluationCriteria
			{
				EvaluationDataKey = completionCriteria.EvaluationDataKey,
				EvaluationDataType = completionCriteria.EvaluationDataType,
				CriteriaQueryType = completionCriteria.CriteriaQueryType,
				ComparisonType = completionCriteria.ComparisonType,
				Scope = completionCriteria.Scope,
				Value = completionCriteria.Value
			};
		}

		public static EvaluationCriteria ToModel(this EvaluationCriteriaUpdateRequest completionCriteria)
		{
			if (completionCriteria == null)
				return null;
			return new EvaluationCriteria
			{
				Id = completionCriteria.Id,
				EvaluationDataKey = completionCriteria.EvaluationDataKey,
				EvaluationDataType = completionCriteria.EvaluationDataType,
				CriteriaQueryType = completionCriteria.CriteriaQueryType,
				ComparisonType = completionCriteria.ComparisonType,
				Scope = completionCriteria.Scope,
				Value = completionCriteria.Value
			};
		}

		public static List<EvaluationCriteria> ToModelList(
			this List<EvaluationCriteriaCreateRequest> completionCriterias)
		{
			return completionCriterias.Select(ToModel)
				.ToList();
		}

		public static List<EvaluationCriteria> ToModelList(
			this List<EvaluationCriteriaUpdateRequest> completionCriterias)
		{
			return completionCriterias.Select(ToModel)
				.ToList();
		}
	}
}