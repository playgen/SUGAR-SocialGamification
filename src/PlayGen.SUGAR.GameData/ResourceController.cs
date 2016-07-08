using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;
using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;

namespace PlayGen.SUGAR.GameData
{
	public class ResourceController
	{
		private IGameDataController _gameDataController;

		public ResourceController(string nameOrConnectionString)
		{
			_gameDataController = new GameDataController(nameOrConnectionString, GameDataCategory.Resource);
		}

		public bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return _gameDataController.KeyExists(gameId, actorId, key, start = default(DateTime), end = default(DateTime));
		}

		public IEnumerable<Data.Model.GameData> Get(int? gameId = null, int? actorId = null, IEnumerable<string> keys = null)
		{
			return _gameDataController.Get(gameId, actorId, keys);
		}

		public void Update(Data.Model.GameData resource)
		{
			_gameDataController.Update(resource);
		}

		public Data.Model.GameData Transfer(int fromResourceId, int? gameId, int? actorId, long transferQuantity, out Data.Model.GameData fromResource)
		{
			fromResource = GetExistingResource(fromResourceId);

			string message;
			if (!IsTransferValid(long.Parse(fromResource.Value), transferQuantity, out message))
			{
				throw new ArgumentException(message);
			}	

			UpdateQuantity(fromResource, -transferQuantity);

			Data.Model.GameData toResource;
			var foundResources = _gameDataController.Get(gameId, actorId, new string[] { fromResource.Key });

			if (foundResources.Any())
			{
				toResource = foundResources.ElementAt(0);
				UpdateQuantity(toResource, transferQuantity);
			}
			else
			{
				toResource = new Data.Model.GameData
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

		public void Create(Data.Model.GameData data)
		{
			var existingEntries = _gameDataController.Get(data.GameId, data.ActorId, new [] {data.Key});
			if (existingEntries.Any())
			{
				throw new DuplicateRecordException();
			}

			_gameDataController.Create(data);
		}

		private void UpdateQuantity(Data.Model.GameData resource, long modifyAmount)
		{
			long currentValue = long.Parse(resource.Value);
			resource.Value = (currentValue + modifyAmount).ToString();

			_gameDataController.Update(resource);
		}

		private Data.Model.GameData GetExistingResource(int fromResourceId)
		{
			var foundResources = _gameDataController.Get(new int[] { fromResourceId });

			if (!foundResources.Any())
			{
				throw new MissingRecordException("No resource with the specified ID was found.");
			}

			return foundResources.Single();
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