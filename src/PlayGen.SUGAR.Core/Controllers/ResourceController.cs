using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class ResourceController
	{
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Data.EntityFramework.Controllers.SaveDataController _saveDataDbController;

		public ResourceController(SUGARContextFactory contextFactory)
		{
			// todo use game data core controller instead of db controller!!!
			_saveDataDbController = new Data.EntityFramework.Controllers.SaveDataController(contextFactory, SaveDataCategory.Resource);
		}

		public bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var result = _saveDataDbController.KeyExists(gameId, actorId, key, start = default(DateTime), end = default(DateTime));

            Logger.Info($"Key Exists: {result} for GameId: {gameId}, ActorId: {actorId}, Key: {key}, Start: {start}, End: {end}");

            return result;
		}

		public List<SaveData> Get(int? gameId = null, int? actorId = null, ICollection<string> keys = null)
		{
			var results = _saveDataDbController.Get(gameId, actorId, keys);

            Logger.Info($"{results?.Count} Game Datas for GameId: {gameId}, ActorId: {actorId}, Keys: {string.Join(", ", keys)}");

            return results;
		}

		public void Update(SaveData resource)
		{
			_saveDataDbController.Update(resource);

            Logger.Info($"{resource.Id}");
        }

		public SaveData Transfer(int? gameId, int? fromActorId, int? toActorId, string key, long transferQuantity, out SaveData fromResource)
		{
			fromResource = GetExistingResource(gameId, fromActorId, key);

			string message;
			if (!IsTransferValid(long.Parse(fromResource.Value), transferQuantity, out message))
			{
				throw new ArgumentException(message);
			}	

			UpdateQuantity(fromResource, -transferQuantity);

			SaveData toResource;
			var foundResources = _saveDataDbController.Get(gameId, toActorId, new List<string> { fromResource.Key });

		    var foundResourceList = foundResources as List<SaveData> ?? foundResources.ToList();
		    if (foundResourceList.Any())
			{
				toResource = foundResourceList.ElementAt(0);
				UpdateQuantity(toResource, transferQuantity);
			}
			else
			{
				toResource = new SaveData
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

            Logger.Info($"{fromResource?.Id} -> {toResource?.Id} for GameId: {gameId}, FromActorId: {fromActorId}, ToActorId: {toActorId}, Key: {key}, Quantity: {transferQuantity}");

			return toResource;
		}

		public void Create(SaveData data)
		{
			var existingEntries = _saveDataDbController.Get(data.GameId, data.ActorId, new List<string>() {data.Key});
			if (existingEntries.Any())
			{
				throw new DuplicateRecordException();
			}
            
			_saveDataDbController.Create(data);

            Logger.Info($"{data?.Id}");
		}

		public void UpdateQuantity(SaveData resource, long modifyAmount)
		{
			long currentValue = long.Parse(resource.Value);
			resource.Value = (currentValue + modifyAmount).ToString();

			_saveDataDbController.Update(resource);

            Logger.Info($"{resource?.Id} with Amount: {modifyAmount}");
        }

		private SaveData GetExistingResource(int? gameId, int? ownerId, string key)
		{
			var foundResources = _saveDataDbController.Get(gameId, ownerId, new List<string> { key });

		    if (!foundResources.Any())
			{
				throw new MissingRecordException("No resource with the specified ID was found.");
			}

		    var found = foundResources.Single();

            Logger.Info($"{found?.Id}");

			return found;
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

			var result = message == string.Empty;

            Logger.Debug($"{result} with Message: \"{message}\" for Current: {current}, Transfer {transfer}");

            return result;
		}
	}
}