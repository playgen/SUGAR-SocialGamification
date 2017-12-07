using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class ResourceExtensions
	{
		public static ResourceResponse ToResourceContract(this EvaluationData evaluationData)
		{
			if (evaluationData == null)
			{
				return null;
			}

			return new ResourceResponse {
				ActorId = evaluationData.ActorId,
				GameId = evaluationData.GameId,
				Key = evaluationData.Key,
				Quantity = long.Parse(evaluationData.Value),
				DateCreated = evaluationData.DateCreated,
				DateModified = evaluationData.DateCreated
			};
		}

		public static IEnumerable<ResourceResponse> ToResourceContractList(this IEnumerable<EvaluationData> gameData)
		{
			return gameData.Select(ToResourceContract).ToList();
		}

		public static EvaluationData ToModel(this ResourceAddRequest resourceContract)
		{
			return new EvaluationData {
				ActorId = resourceContract.ActorId.Value,
				GameId = resourceContract.GameId.Value,
				Key = resourceContract.Key,
				Value = resourceContract.Quantity.ToString(),
				EvaluationDataType = EvaluationDataType.Long,
				Category = EvaluationDataCategory.Resource
			};
		}
	}
}
