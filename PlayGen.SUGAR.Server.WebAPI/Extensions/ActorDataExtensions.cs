using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class ActorDataExtensions
	{
		public static EvaluationDataResponse ToContract(this ActorData actorData)
		{
			if (actorData == null)
			{
				return null;
			}

			return new EvaluationDataResponse
			{
				CreatingActorId = actorData.ActorId,
				GameId = actorData.GameId,
				Key = actorData.Key,
				Value = actorData.Value,
				EvaluationDataType = actorData.EvaluationDataType
			};
		}

		public static IEnumerable<EvaluationDataResponse> ToContractList(this IEnumerable<ActorData> actorDatas)
		{
			return actorDatas.Select(ToContract).ToList();
		}

		public static ActorData ToActorDataModel(this EvaluationDataRequest dataContract)
		{
			return new ActorData
			{
				ActorId = dataContract.CreatingActorId.Value,
				GameId = dataContract.GameId.Value,
				Key = dataContract.Key,
				Value = dataContract.Value,
				EvaluationDataType = dataContract.EvaluationDataType.Value
			};
		}
	}
}