using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	public static class ResourceExtensions
	{
		public static ResourceResponse ToResourceContract(this EvaluationData evaluationData)
		{
			if (evaluationData == null)
			{
				return null;
			}

			return new ResourceResponse {
				Id = evaluationData.Id,
				ActorId = evaluationData.ActorId,
				GameId = evaluationData.GameId,
				Key = evaluationData.Key,
				Quantity = long.Parse(evaluationData.Value),
			};
		}

		public static IEnumerable<ResourceResponse> ToResourceContractList(this IEnumerable<EvaluationData> gameData)
		{
			return gameData.Select(ToResourceContract).ToList();
		}

		public static EvaluationData ToModel(this ResourceAddRequest resourceContract)
		{
			return new EvaluationData {
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
