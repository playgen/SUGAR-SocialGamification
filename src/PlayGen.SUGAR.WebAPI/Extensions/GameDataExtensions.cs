using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class GameDataExtensions
	{
		public static SaveDataResponse ToContract(this Data.Model.SaveData saveData)
		{
			if (saveData == null)
			{
				return null;
			}

			return new SaveDataResponse
			{
				ActorId = saveData.ActorId,
				GameId = saveData.GameId,
				Key = saveData.Key,
				Value = saveData.Value,
				SaveDataType = saveData.SaveDataType
			};
		}

		public static IEnumerable<SaveDataResponse> ToContractList(this IEnumerable<Data.Model.SaveData> gameDatas)
		{
			return gameDatas.Select(ToContract).ToList();
		}

		public static Data.Model.SaveData ToGameDataModel(this SaveDataRequest dataContract)
		{
			return new Data.Model.SaveData
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