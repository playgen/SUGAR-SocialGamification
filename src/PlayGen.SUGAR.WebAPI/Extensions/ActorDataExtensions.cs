using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class ActorDataExtensions
	{
		public static SaveDataResponse ToContract(this Data.Model.ActorData actorData)
		{
			if (actorData == null)
			{
				return null;
			}

			return new SaveDataResponse
			{
				ActorId = actorData.ActorId,
				GameId = actorData.GameId,
				Key = actorData.Key,
				Value = actorData.Value,
				SaveDataType = actorData.SaveDataType
			};
		}

		public static IEnumerable<SaveDataResponse> ToContractList(this IEnumerable<Data.Model.ActorData> actorDatas)
		{
			return actorDatas.Select(ToContract).ToList();
		}

		public static Data.Model.ActorData ToActorDataModel(this SaveDataRequest dataContract)
		{
			return new Data.Model.ActorData
			{
				ActorId = dataContract.ActorId,
				GameId = dataContract.GameId,
				Key = dataContract.Key,
				Value = dataContract.Value,
				SaveDataType = dataContract.SaveDataType
			};
		}
	}
}