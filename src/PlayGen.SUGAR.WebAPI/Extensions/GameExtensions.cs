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
			{
				return null;
			}
			var gameContract = new GameResponse
			{
				Id = gameModel.Id,
				Name = gameModel.Name
			};

			return gameContract;
		}

		public static IEnumerable<GameResponse> ToContractList(this IEnumerable<Game> gameModels)
		{
			return gameModels.Select(ToContract).ToList();
		}

		public static Game ToModel(this GameRequest gameContract)
		{
			var gameModel = new Game { Name = gameContract.Name };

			return gameModel;
		}

	}
}