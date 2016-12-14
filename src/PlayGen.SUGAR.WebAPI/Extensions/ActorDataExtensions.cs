using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class ActorDataExtensions
	{
		public static EvaluationDataResponse ToContract(this Data.Model.ActorData actorData)
		{
			if (actorData == null)
			{
				return null;
			}

			return new EvaluationDataResponse
			{
				ActorId = actorData.ActorId,
				GameId = actorData.GameId,
				Key = actorData.Key,
				Value = actorData.Value,
				EvaluationDataType = actorData.EvaluationDataType
			};
		}

		public static IEnumerable<EvaluationDataResponse> ToContractList(this IEnumerable<Data.Model.ActorData> actorDatas)
		{
			return actorDatas.Select(ToContract).ToList();
		}

		public static Data.Model.ActorData ToActorDataModel(this EvaluationDataRequest dataContract)
		{
			return new Data.Model.ActorData
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