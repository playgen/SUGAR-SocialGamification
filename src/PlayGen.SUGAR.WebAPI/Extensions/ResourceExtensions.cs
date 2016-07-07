using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public  static class ResourceExtensions
    {
		public static ResourceResponse ToResourceContract(this Data.Model.GameData gameData)
		{
			return new ResourceResponse
			{
				ActorId = gameData.ActorId,
				GameId = gameData.GameId,
				Key = gameData.Key,
				Quantity = long.Parse(gameData.Value),
			};
		}

		public static IEnumerable<ResourceResponse> ToResourceContractList(this IEnumerable<Data.Model.GameData> gameData)
		{
			return gameData.Select(ToResourceContract).ToList();
		}

		public static Data.Model.GameData ToModel(this ResourceRequest resourceContract)
		{
			return new Data.Model.GameData
			{
				ActorId = resourceContract.ActorId,
				GameId = resourceContract.GameId,
				Key = resourceContract.Key,
				Value = resourceContract.Quantity.ToString(),
				DataType = GameDataType.Long,
				Category = GameDataCategory.Resource
			};
		}
    }
}
