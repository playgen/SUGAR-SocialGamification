using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public  static class ResourceExtensions
	{
		public static ResourceResponse ToResourceContract(this SaveData saveData)
		{
			if (saveData == null)
			{
				return null;
			}

			return new ResourceResponse
			{
				Id = saveData.Id,
				ActorId = saveData.ActorId,
				GameId = saveData.GameId,
				Key = saveData.Key,
				Quantity = long.Parse(saveData.Value),
			};
		}

		public static IEnumerable<ResourceResponse> ToResourceContractList(this IEnumerable<SaveData> gameData)
		{
			return gameData.Select(ToResourceContract).ToList();
		}

		public static SaveData ToModel(this ResourceAddRequest resourceContract)
		{
			return new SaveData
			{
				ActorId = resourceContract.ActorId,
				GameId = resourceContract.GameId,
				Key = resourceContract.Key,
				Value = resourceContract.Quantity.ToString(),
				SaveDataType = SaveDataType.Long,
				Category = SaveDataCategory.Resource
			};
		}
	}
}
