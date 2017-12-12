using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	public static class EvaluationDataExtensions
	{
		public static List<EvaluationDataResponse> ToContractList(this List<EvaluationData> models)
		{
			return models.Select(ToContract).ToList();
		}

		public static EvaluationDataResponse ToContract(this EvaluationData evaluationData)
		{
			if (evaluationData == null)
			{
				return null;
			}

			return new EvaluationDataResponse {
				GameId = evaluationData.GameId,
				MatchId = evaluationData.MatchId,
				CreatingActorId = evaluationData.ActorId,
				Key = evaluationData.Key,
				Value = evaluationData.Value,
				EvaluationDataType = evaluationData.EvaluationDataType,
				DateCreated = evaluationData.DateCreated == default(DateTime) ? (DateTime?)null : evaluationData.DateCreated,
				DateModified = evaluationData.DateModified == default(DateTime) ? (DateTime?)null : evaluationData.DateCreated
			};
		}
	}
}