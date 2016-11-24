using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class GameDataExtensions
	{
		public static SaveDataResponse ToContract(this Data.Model.GameData gameData)
		{
			if (gameData == null)
			{
				return null;
			}

			return new SaveDataResponse
			{
				ActorId = gameData.ActorId,
				GameId = gameData.GameId,
				Key = gameData.Key,
				Value = gameData.Value,
				SaveDataType = gameData.SaveDataType
			};
		}

		public static IEnumerable<SaveDataResponse> ToContractList(this IEnumerable<Data.Model.GameData> gameDatas)
		{
			return gameDatas.Select(ToContract).ToList();
		}

		public static Data.Model.GameData ToGameDataModel(this SaveDataRequest dataContract)
		{
			return new Data.Model.GameData
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