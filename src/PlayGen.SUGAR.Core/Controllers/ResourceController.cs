using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class ResourceController
	{
		private readonly Data.EntityFramework.Controllers.GameDataController _gameDataDbController;

		public ResourceController(SUGARContextFactory contextFactory)
		{
			// todo use game data core controller instead of db controller!!!
			_gameDataDbController = new Data.EntityFramework.Controllers.GameDataController(contextFactory, GameDataCategory.Resource);
		}

		public bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return _gameDataDbController.KeyExists(gameId, actorId, key, start = default(DateTime), end = default(DateTime));
		}

		public IEnumerable<GameData> Get(int? gameId = null, int? actorId = null, IEnumerable<string> keys = null)
		{
			return _gameDataDbController.Get(gameId, actorId, keys);
		}

		public void Update(GameData resource)
		{

			_gameDataDbController.Update(resource);
		}

		public GameData Transfer(int? gameId, int? fromActorId, int? toActorId, string key, long transferQuantity, out GameData fromResource)
		{
			fromResource = GetExistingResource(gameId, fromActorId, key);

			string message;
			if (!IsTransferValid(long.Parse(fromResource.Value), transferQuantity, out message))
			{
				throw new ArgumentException(message);
			}	

			UpdateQuantity(fromResource, -transferQuantity);

			GameData toResource;
			var foundResources = _gameDataDbController.Get(gameId, toActorId, new[] { fromResource.Key });

		    var foundResourceList = foundResources as List<GameData> ?? foundResources.ToList();
		    if (foundResourceList.Any())
			{
				toResource = foundResourceList.ElementAt(0);
				UpdateQuantity(toResource, transferQuantity);
			}
			else
			{
				toResource = new GameData
				{
					GameId = gameId,
					ActorId = toActorId,
					Key = fromResource.Key,
					Value = transferQuantity.ToString(),
					Category = fromResource.Category,
					SaveDataType = fromResource.SaveDataType,
				};
				Create(toResource);
			}

			return toResource;
		}

		public void Create(GameData data)
		{
			var existingEntries = _gameDataDbController.Get(data.GameId, data.ActorId, new [] {data.Key});
			if (existingEntries.Any())
			{
				throw new DuplicateRecordException();
			}

			_gameDataDbController.Create(data);
		}

		public void UpdateQuantity(GameData resource, long modifyAmount)
		{
			long currentValue = long.Parse(resource.Value);
			resource.Value = (currentValue + modifyAmount).ToString();

			_gameDataDbController.Update(resource);
		}

		private GameData GetExistingResource(int? gameId, int? ownerId, string key)
		{
			var foundResources = _gameDataDbController.Get(gameId, ownerId, new []{ key });

		    var foundResourceList = foundResources as List<GameData> ?? foundResources.ToList();
		    if (!foundResourceList.Any())
			{
				throw new MissingRecordException("No resource with the specified ID was found.");
			}

			return foundResourceList.Single();
		}

		private bool IsTransferValid(long current, long transfer, out string message)
		{
			message = string.Empty;

			if (current < transfer)
			{
				message = "The quantity to transfer cannot be greater than the quantity available to transfer.";
			}
			else if(transfer < 1)
			{
				message = "The quantity to transfer cannot be less than one.";
			}

			return message == string.Empty;
		}
	}
}