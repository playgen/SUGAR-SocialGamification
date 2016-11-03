using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class GameDataExtensions
	{
		public static GameDataResponse ToContract(this Data.Model.GameData gameData)
		{
			if (gameData == null)
			{
				return null;
			}

            return new GameDataResponse
			{
				ActorId = gameData.ActorId,
				GameId = gameData.GameId,
				Key = gameData.Key,
				Value = gameData.Value,
				GameDataType = gameData.DataType
			};
		}

		public static IEnumerable<GameDataResponse> ToContractList(this IEnumerable<Data.Model.GameData> gameDatas)
		{
			return gameDatas.Select(ToContract).ToList();
		}

		public static Data.Model.GameData ToModel(this GameDataRequest dataContract)
		{
			return new Data.Model.GameData
			{
				ActorId = dataContract.ActorId,
				GameId = dataContract.GameId,
				Key = dataContract.Key,
				Value = dataContract.Value,
				DataType = dataContract.GameDataType
			};
		}
	}
}