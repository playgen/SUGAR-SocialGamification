using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Core.Exceptions;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class EvaluationDataController
	{
		public static Action<EvaluationData> EvaluationDataAddedEvent;

		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.EvaluationDataController _evaluationDataDbController;
		private readonly EvaluationDataCategory _category;

		public EvaluationDataController(
			ILogger<EvaluationDataController> logger,
			SUGARContextFactory contextFactory, 
			EvaluationDataCategory category)
		{
			_logger = logger;
			_category = category;
			_evaluationDataDbController = new EntityFramework.Controllers.EvaluationDataController(contextFactory, category);
		}

		public void Add(List<EvaluationData> datas)
		{
			datas.ForEach(ValidateData);

            _evaluationDataDbController.Create(datas);
			
			datas.ForEach(d => EvaluationDataAddedEvent?.Invoke(d));

			_logger.LogInformation($"Added: {datas.Count} Evaluation Datas.");
        }

		public EvaluationData Add(EvaluationData newData)
		{
			ValidateData(newData);

			newData = _evaluationDataDbController.Create(newData);

			_logger.LogInformation($"{newData?.Id}");

			EvaluationDataAddedEvent?.Invoke(newData);

			return newData;
		}

		public EvaluationData Update(EvaluationData data)
		{
			ValidateData(data);

			data = _evaluationDataDbController.Update(data);

			_logger.LogInformation($"Updated: {data?.Id}");

			return data;
		}

		public bool KeyExists(int gameId, int actorId, string key, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var keyExists = _evaluationDataDbController.KeyExists(gameId, actorId, key, evaluationDataType, start, end);

			_logger.LogDebug(
				$"Key Exists: {keyExists} for GameId: {gameId}, ActorId: {actorId}, Key: {key}, EvaludationDataCategory: {_category}, Start: {start}, End: {end}");

			return keyExists;
		}

		public List<EvaluationData> Get(ICollection<int> ids)
		{
			var datas = _evaluationDataDbController.Get(ids);

			_logger.LogInformation($"{datas.Count} Evaluation Datas for Ids: {string.Join(", ", ids)}");

			return datas;
		}

		public List<EvaluationData> Get(int entityId, string[] keys = null)
		{
			var datas = _evaluationDataDbController.Get(entityId, keys);

			_logger.LogInformation($"{datas?.Count} Evaluation Datas for EntityId {entityId}" +
						(keys != null ? $", Keys: {string.Join(", ", keys)}" : ""));

			return datas;
		}

		public List<EvaluationData> Get(int gameId, int actorId, string[] keys = null)
		{
			var datas = _evaluationDataDbController.Get(gameId, actorId, keys);

			_logger.LogInformation($"{datas?.Count} Evaluation Datas for GameId: {gameId}, ActorId {actorId}" +
						(keys != null ? $", Keys: {string.Join(", ", keys)}" : ""));

			return datas;
		}

		public List<EvaluationData> Get(int gameId, int actorId, int? entityId, string[] keys = null)
		{
			var datas = _evaluationDataDbController.Get(gameId, actorId, entityId, keys);

			_logger.LogInformation($"{datas?.Count} Evaluation Datas for GameId: {gameId}, EntityId: {entityId}, ActorId {actorId}" +
						(keys != null ? $", Keys: {string.Join(", ", keys)}" : ""));

			return datas;
		}

		public EvaluationData Get(int gameId, int actorId, string key, EvaluationDataType dataType, LeaderboardType sortType)
		{
			if (((int)sortType == 3 && ((int)dataType == 1 || (int)dataType == 2)) || ((int)sortType < 3 && ((int)dataType == 0 || (int)dataType == 3)))
			{
				throw new ArgumentException($"Cannot get EvaluationData for LeaderboardType {sortType} and EvaluationDataType {dataType} as it would always return zero results.");
			}
			EvaluationData data = null;
			string value = null;
			switch (sortType)
			{
				case LeaderboardType.Highest:
					TryGetMax(gameId, actorId, key, out data, dataType);
					break;
				case LeaderboardType.Lowest:
					TryGetMin(gameId, actorId, key, out data, dataType);
					break;
				case LeaderboardType.Cumulative:
					switch (dataType)
					{
						case EvaluationDataType.Long:
							value = SumLong(gameId, actorId, key).ToString(CultureInfo.InvariantCulture);
							break;
						case EvaluationDataType.Float:
							value = SumFloat(gameId, actorId, key).ToString(CultureInfo.InvariantCulture);
							break;
					}
					break;
				case LeaderboardType.Count:
					value = CountKeys(gameId, actorId, key, dataType).ToString(CultureInfo.InvariantCulture);
					break;
				case LeaderboardType.Earliest:
					TryGetEarliest(gameId, actorId, key, out data, dataType);
					break;
				case LeaderboardType.Latest:
					TryGetLatest(gameId, actorId, key, out data, dataType);
					break;
			}

			data = new EvaluationData
			{
				ActorId = actorId,
				GameId = gameId,
				Category = _category,
				EvaluationDataType = dataType,
				Key = key,
				Value = value
			};

			return data;
		}

		public List<int> GetGameActors(int gameId)
		{
			var actors = _evaluationDataDbController.GetGameActors(gameId);

			_logger.LogInformation($"{actors?.Count} Actors for GameId {gameId}");

			return actors;
		}

		public List<int> GetGameKeyActors(int gameId, string key, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var actors = _evaluationDataDbController.GetGameKeyActors(gameId, key, evaluationDataType, start, end);

			_logger.LogInformation($"{actors?.Count} Actors for GameId {gameId} and Key {key}");

			return actors;
		}

		public List<KeyValuePair<string, EvaluationDataType>> GetGameKeys(int gameId)
		{
			var datas = _evaluationDataDbController.GetGameKeys(gameId);

			_logger.LogInformation($"{datas?.Count} Evaluation Data Keys for GameId {gameId}");

			return datas;
		}

		public List<EvaluationData> GetActorData(int actorId)
		{
			var datas = _evaluationDataDbController.GetActorData(actorId);

			_logger.LogInformation($"{datas?.Count} Evaluation Data Keys for ActorId {actorId}");

			return datas;
		}

		public List<EvaluationData> List(int gameId, int actorId, string key, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var list = _evaluationDataDbController.List(gameId, actorId, key, evaluationDataType, start, end);

			_logger.LogDebug($"{list?.Count} Values for: GameId: {gameId}, ActorId {actorId}, Key: {key}, EvaluationDataType: {evaluationDataType}, Start: {start}, End: {end}");

			return list;
		}

		public float SumFloat(int gameId, int actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var sum = _evaluationDataDbController.SumFloat(gameId, actorId, key, start, end);

			_logger.LogDebug($"{nameof(SumFloat)}: {sum} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return sum;
		}

        public long SumLong(int gameId, int actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var sum = _evaluationDataDbController.SumLong(gameId, actorId, key, start, end);

			_logger.LogDebug($"{nameof(SumLong)}: {sum} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return sum;
		}

		public bool TryGetMax(int gameId, int actorId, string key, out EvaluationData value, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var dataFound = _evaluationDataDbController.TryGetMax(gameId, actorId, key, out value, evaluationDataType, start, end);

			_logger.LogDebug($"Highest: {value?.ToString() ?? "Null"} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, EvaluationDataType: {evaluationDataType}, Start: {start}, End: {end}");

			return dataFound;
		}

		public bool TryGetMin(int gameId, int actorId, string key, out EvaluationData value, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var dataFound = _evaluationDataDbController.TryGetMin(gameId, actorId, key, out value, evaluationDataType, start, end);

			_logger.LogDebug($"Lowest: {value?.ToString() ?? "Null"} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, EvaluationDataType: {evaluationDataType}, Start: {start}, End: {end}");

			return dataFound;
		}

		public bool TryGetLatest(int gameId, int actorId, string key, out EvaluationData latest, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var didGetLatest = _evaluationDataDbController.TryGetLatest(gameId, actorId, key, out latest, evaluationDataType, start, end);

			_logger.LogDebug($"Latest {latest?.ToString() ?? "Null"} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, EvaluationDataType: {evaluationDataType}, Start: {start}, End: {end}");

			return didGetLatest;
		}

		public bool TryGetEarliest(int gameId, int actorId, string key, out EvaluationData earliest, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var didGetEarliest = _evaluationDataDbController.TryGetEarliest(gameId, actorId, key, out earliest, evaluationDataType, start, end);

			_logger.LogDebug($"Earliest {earliest?.ToString() ?? "Null"} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, EvaluationDataType: {evaluationDataType}, Start: {start}, End: {end}");

			return didGetEarliest;
		}

		public int CountKeys(int gameId, int actorId, string key, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var count = _evaluationDataDbController.CountKeys(gameId, actorId, key, evaluationDataType, start, end);

			_logger.LogDebug($"Count: {count} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, EvaluationDataType: {evaluationDataType}, Start: {start}, End: {end}");

			return count;
		}

		protected static bool ParseCheck(EvaluationData data)
		{
			switch (data.EvaluationDataType)
			{
				case EvaluationDataType.String:
					return true;

				case EvaluationDataType.Long:
					return long.TryParse(data.Value, out _);

				case EvaluationDataType.Float:
					return float.TryParse(data.Value, out _);

				case EvaluationDataType.Boolean:
					return bool.TryParse(data.Value, out _);

				default:
					return false;
			}
		}

		protected void ValidateData(EvaluationData data)
		{
			if (data.Category != _category)
			{
				throw new InvalidDataException(
					$"Cannot save datas with category: {data.Category} with controller for mismatched category: {_category}");
			}

			if (!IsValid(data, out var failure))
			{
				throw new ArgumentException(failure);
			}
		}

		public static bool IsValid(EvaluationData data, out string failure)
		{
			failure = null;

			if (!RegexUtil.IsAlphaNumericUnderscoreNotEmpty(data.Key))
			{
				failure = $"Invalid Key {data.Key}. Keys must be non-empty strings containing only alpha-numeric characters and underscores.";
			} 
			else if (!ParseCheck(data))
			{
				failure = $"Invalid Value {data.Value} for EvaluationDataType {data.EvaluationDataType}";
			}

			return failure == null;
		}
	}
}