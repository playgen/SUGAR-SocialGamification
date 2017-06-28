using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class ResourceExtensions
	{
		public static ResourceResponse ToContract(this EvaluationData evaluationData)
		{
			if (evaluationData == null)
				return null;

			return new ResourceResponse
			{
				Id = evaluationData.Id,
				ActorId = evaluationData.ActorId,
				GameId = evaluationData.GameId,
				Key = evaluationData.Key,
				Quantity = long.Parse(evaluationData.Value)
			};
		}

		public static ResourcesResponse ToCollectionContract(this IEnumerable<EvaluationData> models)
		{
			return new ResourcesResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}

		public static EvaluationData ToModel(this ResourceAddRequest resourceContract)
		{
			return new EvaluationData
			{
				ActorId = resourceContract.ActorId,
				GameId = resourceContract.GameId,
				Key = resourceContract.Key,
				Value = resourceContract.Quantity.ToString(),
				EvaluationDataType = EvaluationDataType.Long,
				Category = EvaluationDataCategory.Resource
			};
		}
	}
}