using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;
/*
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

	    public void Update(GameData resource)
	    {
		    _gameDataController.Update(resource);
	    }

	    public GameData Transfer(int fromResourceId, int? gameId, int? actorId, long transferQuantity, out GameData fromResource)
	    {
		    fromResource = GetExistingResource(fromResourceId);
		    UpdateQuantity(fromResource, -transferQuantity);

			GameData toResource;
			var foundResources = _gameDataController.Get(gameId, actorId, new string[] { fromResource.Key });

		    if (foundResources.Any())
		    {
				toResource = foundResources.ElementAt(0);
				UpdateQuantity(toResource, transferQuantity);
		    }
		    else
		    {
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

	    private void UpdateQuantity(GameData resource, long modifyAmount)
	    {
			long currentValue = long.Parse(resource.Value);
			resource.Value = (currentValue + modifyAmount).ToString();

			_gameDataController.Update(resource);
		}

	    private GameData GetExistingResource(int fromResourceId)
	    {
		    var foundResources = _gameDataController.Get(new int[] {fromResourceId});

		    if (!foundResources.Any())
		    {
			    throw new MissingRecordException("No resource with the specified ID was found.");
		    }

		    return foundResources.Single();
	    }
    }
}
*/