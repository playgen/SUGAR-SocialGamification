using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;
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

            return new GameResponse
			{
				Id = gameModel.Id,
				Name = gameModel.Name
			};
		}

		public static IEnumerable<GameResponse> ToContractList(this IEnumerable<Game> gameModels)
		{
			return gameModels.Select(ToContract).ToList();
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