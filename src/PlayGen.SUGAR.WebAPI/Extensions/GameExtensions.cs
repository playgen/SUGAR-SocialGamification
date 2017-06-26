using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class GameExtensions
	{
		public static GameResponse ToContract(this Game gameModel)
		{
			if (gameModel == null)
				return null;

			return new GameResponse
			{
				Id = gameModel.Id,
				Name = gameModel.Name
			};
		}

		public static CollectionResponse ToCollectionContract(this IEnumerable<Game> models)
		{
			return new CollectionResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}

		public static Game ToModel(this GameRequest gameContract)
		{
			return new Game
			{
				Name = gameContract.Name
			};
		}
	}
}