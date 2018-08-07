using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class ResourceController
	{
		private readonly ILogger _logger;
		private readonly EvaluationDataController _evaluationDataController;

		public ResourceController(
			ILogger<ResourceController> logger,
			ILogger<EvaluationDataController> evaluationDataLogger,
			SUGARContextFactory contextFactory)
		{
			_logger = logger;
			_evaluationDataController = new EvaluationDataController(evaluationDataLogger, contextFactory, EvaluationDataCategory.Resource);
		}

		public List<EvaluationData> Get(int gameId, int actorId, string[] keys = null)
		{
			var results = _evaluationDataController.Get(gameId, actorId, keys);

			return results;
		}

		public EvaluationData Transfer(int gameId, int fromActorId, int toActorId, string key, long transferQuantity, out EvaluationData fromResource)
		{
			fromResource = GetExistingResource(gameId, fromActorId, key);

			if (!IsTransferValid(long.Parse(fromResource.Value), transferQuantity, out var message))
			{
				throw new ArgumentException(message);
			}

			fromResource = AddQuantity(fromResource.Id, -transferQuantity);

			EvaluationData toResource;
			var foundResources = _evaluationDataController.Get(gameId, toActorId, new[] { fromResource.Key });

			if (foundResources.Any())
			{
				toResource = foundResources.Single();
				toResource = AddQuantity(toResource.Id, transferQuantity);
			}
			else
			{
				toResource = new EvaluationData {
					GameId = gameId,
					ActorId = toActorId,
					Key = fromResource.Key,
					Value = transferQuantity.ToString(),
					Category = fromResource.Category,
					EvaluationDataType = fromResource.EvaluationDataType
				};
				Create(toResource);
			}

			_logger.LogInformation($"{fromResource?.Id} -> {toResource?.Id} for GameId: {gameId}, FromActorId: {fromActorId}, ToActorId: {toActorId}, Key: {key}, Quantity: {transferQuantity}");

			return toResource;
		}

		public void Create(EvaluationData data)
		{
			if (!EvaluationDataController.IsValid(data, out var failure))
			{
				throw new ArgumentException(failure);
			}
			
			var existingEntries = _evaluationDataController.Get(data.GameId, data.ActorId, new[] {data.Key});

			if (existingEntries.Any())
			{
				throw new DuplicateRecordException();
			}

			_evaluationDataController.Add(data);

			_logger.LogInformation($"{data.Id}");
		}

		public EvaluationData AddQuantity(int resourceId, long addAmount)
		{
			var resource = _evaluationDataController.Get(new[] { resourceId }).Single();

			var value = long.Parse(resource.Value) + addAmount;
			if (value < 0)
			{
				value = 0;
			}
			resource.Value = (value).ToString();

			_evaluationDataController.Update(resource);

			_logger.LogInformation($"{resource.Id} with Amount: {addAmount}");

			return resource;
		}

		public EvaluationData CreateOrUpdate(int gameId, int actorId, string key, long quantity)
		{
			EvaluationData resource;

			var resources = Get(gameId, actorId, new[] { key });
			if (resources.Any())
			{
				// todo handle edge case of multiple resource entries
				var existingResource = resources.Single();
				resource = AddQuantity(existingResource.Id, quantity);
			}
			else
			{
				resource = new EvaluationData
				{
					GameId = gameId,
					ActorId = actorId,
					Key = key,
					Value = quantity.ToString(),
                    EvaluationDataType = EvaluationDataType.Long,
					Category = EvaluationDataCategory.Resource,
				};

				Create(resource);
			}

			return resource;
		}

        private EvaluationData GetExistingResource(int gameId, int ownerId, string key)
		{
			var foundResources = _evaluationDataController.Get(gameId, ownerId, new[] { key });

			if (!foundResources.Any())
			{
				throw new MissingRecordException("No resource with the specified ID was found.");
			}

			var found = foundResources.Single();

			_logger.LogInformation($"{found?.Id}");

			return found;
		}

		private bool IsTransferValid(long current, long transfer, out string message)
		{
			message = string.Empty;

			if (current < transfer)
			{
				message = "The quantity to transfer cannot be greater than the quantity available to transfer.";
			}
			else if (transfer < 1)
			{
				message = "The quantity to transfer cannot be less than one.";
			}

			var result = message == string.Empty;

			_logger.LogDebug($"{result} with Message: \"{message}\" for Current: {current}, Transfer {transfer}");

			return result;
		}
	}
}