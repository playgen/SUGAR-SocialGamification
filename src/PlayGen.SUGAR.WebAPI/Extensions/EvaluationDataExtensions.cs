using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class EvaluationDataExtensions
	{
		public static CollectionResponse ToCollectionContract(this List<EvaluationData> models)
		{
			return new CollectionResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}

		public static EvaluationDataResponse ToContract(this EvaluationData evaluationData)
		{
			if (evaluationData == null)
				return null;

			return new EvaluationDataResponse
			{
				GameId = evaluationData.GameId,
				MatchId = evaluationData.MatchId,
				CreatingActorId = evaluationData.ActorId,
				Key = evaluationData.Key,
				Value = evaluationData.Value,
				EvaluationDataType = evaluationData.EvaluationDataType
			};
		}
	}
}