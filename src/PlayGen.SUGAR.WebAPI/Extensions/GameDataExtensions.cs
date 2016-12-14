using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class GameDataExtensions
	{
		public static EvaluationDataResponse ToContract(this Data.Model.EvaluationData evaluationData)
		{
			if (evaluationData == null)
			{
				return null;
			}

			return new EvaluationDataResponse
			{
				ActorId = evaluationData.ActorId,
				GameId = evaluationData.GameId,
				Key = evaluationData.Key,
				Value = evaluationData.Value,
				EvaluationDataType = evaluationData.EvaluationDataType
			};
		}

		public static IEnumerable<EvaluationDataResponse> ToContractList(this IEnumerable<Data.Model.EvaluationData> gameDatas)
		{
			return gameDatas.Select(ToContract).ToList();
		}

		public static Data.Model.EvaluationData ToGameDataModel(this EvaluationDataRequest dataContract)
		{
			return new Data.Model.EvaluationData
			{
				ActorId = dataContract.ActorId,
				GameId = dataContract.GameId,
				Key = dataContract.Key,
				Value = dataContract.Value,
				EvaluationDataType = dataContract.EvaluationDataType
			};
		}
	}
}