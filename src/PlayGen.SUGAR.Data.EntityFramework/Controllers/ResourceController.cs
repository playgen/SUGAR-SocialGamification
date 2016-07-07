using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
    public class ResourceController
    {
	    private GameDataController _gameDataController;

		public ResourceController(string nameOrConnectionString)
		{
			_gameDataController = new GameDataController(nameOrConnectionString, GameDataCategory.Resource);
		}

		public bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return _gameDataController.KeyExists(gameId, actorId, key, start = default(DateTime), end = default(DateTime));
		}

		public IEnumerable<GameData> Get(int? gameId = null, int? actorId = null, IEnumerable<string> keys = null)
		{
			return _gameDataController.Get(gameId, actorId, keys);
		}

	    public GameData Transfer(int fromResourceId, int? gameId, int? actorId, long transferQuantity, out GameData fromResource)
	    {
		    var foundResources = _gameDataController.Get(new int[] {fromResourceId});

		    if (!foundResources.Any())
		    {
			    throw new MissingRecordException("No resource with the specified ID was found.");
		    }

		    fromResource = foundResources.Single();
		    var fromQuantity = long.Parse(fromResource.Value);


			if (fromQuantity < transferQuantity)
		    {
			    throw new ArgumentException("The transferQuantity to transfer cannot be greater than the resource's current transferQuantity");
		    }

		    foundResources = _gameDataController.Get(gameId, actorId, new string[] {fromResource.Key});

			// Deduct from fromResource
		    fromResource.Value = (fromQuantity - transferQuantity).ToString();
			_gameDataController.Update(fromResource);

			// Add to toResource
			GameData toResource;

		    if (foundResources.Any())
		    {
				// Update
			    toResource = foundResources.ElementAt(0);
				var toQuantity = long.Parse(toResource.Value);
			    toResource.Value = (toQuantity + transferQuantity).ToString();
				_gameDataController.Update(toResource);
		    }
		    else
		    {
			    // Create
			    toResource = new GameData
			    {
				    GameId = gameId,
				    ActorId = actorId,
				    Value = transferQuantity.ToString(),
				    Category = fromResource.Category,
				    DataType = fromResource.DataType,
			    };

				Create(toResource);
		    }

		    return toResource;
	    }

		public void Create(GameData data)
		{
			_gameDataController.Create(data);
		}
	}
}
