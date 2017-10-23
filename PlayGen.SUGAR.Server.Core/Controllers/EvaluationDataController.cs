using System;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;
using NLog;
using PlayGen.SUGAR.Core.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class EvaluationDataController
	{
		public static Action<EvaluationData> EvaluationDataAddedEvent;

		private static Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Data.EntityFramework.Controllers.EvaluationDataController _evaluationDataDbController;
		private readonly EvaluationDataCategory _category;

		public EvaluationDataController(SUGARContextFactory contextFactory, EvaluationDataCategory category)
		{
			_category = category;
			_evaluationDataDbController = new Data.EntityFramework.Controllers.EvaluationDataController(contextFactory, category);
		}

		public void Add(EvaluationData[] datas)
		{
			var dataList = new List<EvaluationData>();
			var i = 0;
			var chunkSize = 1000;

			var uniqueAddedData = new Dictionary<string, EvaluationData>();

			do
			{
				var newData = datas[i];
				ValidateData(newData);
				dataList.Add(newData);
				uniqueAddedData[$"{newData.GameId}_{newData.Key}_{newData.Category}"] = newData;

				if (dataList.Count >= chunkSize || i == datas.Length - 1)
				{
					_evaluationDataDbController.Create(dataList);
					dataList.Clear();
				}

				i++;

			} while (dataList.Count > 0 && i < datas.Length);

			foreach (var addedData in uniqueAddedData.Values)
			{
				EvaluationDataAddedEvent?.Invoke(addedData);
			}

			Logger.Info($"Added: {datas?.Length} Evaluation Datas.");
		}

		public EvaluationData Add(EvaluationData newData)
		{
			ValidateData(newData);

			if (ParseCheck(newData))
			{
				newData = _evaluationDataDbController.Create(newData);

				Logger.Info($"{newData?.Id}");

				EvaluationDataAddedEvent?.Invoke(newData);

				return newData;
			}
			else
			{
				throw new ArgumentException($"Invalid Value {newData.Value} for EvaluationDataType {newData.EvaluationDataType}");
			}
		}

		public EvaluationData Update(EvaluationData data)
		{
			ValidateData(data);

			data = _evaluationDataDbController.Update(data);

			Logger.Info($"Updated: {data?.Id}");

			return data;
		}

		public bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime),
			DateTime end = default(DateTime))
		{
			var keyExists = _evaluationDataDbController.KeyExists(gameId, actorId, key, start, end);

			Logger.Debug(
				$"Key Exists: {keyExists} for GameId: {gameId}, ActorId: {actorId}, Key: {key}, Start: {start}, End: {end}");

			return keyExists;
		}

		public List<EvaluationData> Get(ICollection<int> ids)
		{
			var datas = _evaluationDataDbController.Get(ids);

			Logger.Info($"{datas.Count} Evaluation Datas for Ids: {string.Join(", ", ids)}");

			return datas;
		}

		public List<EvaluationData> Get(int entityId, string[] keys = null)
		{
			var datas = _evaluationDataDbController.Get(entityId, keys);

			Logger.Info($"{datas?.Count} Evaluation Datas for EntityId {entityId}" +
						(keys != null ? $", Keys: {string.Join(", ", keys)}" : ""));

			return datas;
		}

		public List<EvaluationData> Get(int? gameId = null, int? actorId = null, string[] keys = null)
		{
			var datas = _evaluationDataDbController.Get(gameId, actorId, keys);

			Logger.Info($"{datas?.Count} Evaluation Datas for GameId: {gameId}, ActorId {actorId}" +
						(keys != null ? $", Keys: {string.Join(", ", keys)}" : ""));

			return datas;
		}

		public List<EvaluationData> Get(int? gameId, int? entityId, int? actorId, string[] keys)
		{
			var datas = _evaluationDataDbController.Get(gameId, entityId, actorId, keys);

			Logger.Info($"{datas?.Count} Evaluation Datas for GameId: {gameId}, EntityId: {entityId}, ActorId {actorId}" +
						(keys != null ? $", Keys: {string.Join(", ", keys)}" : ""));

			return datas;
		}

		public List<int?> GetGameActors(int? gameId)
		{
			var actors = _evaluationDataDbController.GetGameActors(gameId);

			Logger.Info($"{actors?.Count} Actors for GameId {gameId}");

			return actors;
		}

		public List<KeyValuePair<string, EvaluationDataType>> GetGameKeys(int? gameId)
		{
			var datas = _evaluationDataDbController.GetGameKeys(gameId);

			Logger.Info($"{datas?.Count} Evaluation Data Keys for GameId {gameId}");

			return datas;
		}

		public List<EvaluationData> GetActorData(int? actorId)
		{
			var datas = _evaluationDataDbController.GetActorData(actorId);

			Logger.Info($"{datas?.Count} Evaluation Data Keys for ActorId {actorId}");

			return datas;
		}

		public List<long> AllLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime),
			DateTime end = default(DateTime))
		{
			var longs = _evaluationDataDbController.AllLongs(gameId, actorId, key, start, end);

			Logger.Debug(
				$"{longs?.Count} Values for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return longs;
		}

		public List<float> AllFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime),
			DateTime end = default(DateTime))
		{
			var floats = _evaluationDataDbController.AllFloats(gameId, actorId, key, start, end);

			Logger.Debug(
				$"{floats?.Count} Values for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return floats;
		}

		public List<string> AllStrings(int? gameId, int? actorId, string key, DateTime start = default(DateTime),
			DateTime end = default(DateTime))
		{
			var strings = _evaluationDataDbController.AllStrings(gameId, actorId, key, start, end);

			Logger.Debug(
				$"{strings?.Count} Values for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return strings;
		}

		public List<bool> AllBools(int? gameId, int? actorId, string key, DateTime start = default(DateTime),
			DateTime end = default(DateTime))
		{
			var bools = _evaluationDataDbController.AllBools(gameId, actorId, key, start, end);

			Logger.Debug(
				$"{bools?.Count} Values for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return bools;
		}

		public float SumFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime),
			DateTime end = default(DateTime))
		{
			var sum = _evaluationDataDbController.SumFloats(gameId, actorId, key, start, end);

			Logger.Debug($"Sum: {sum} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return sum;
		}

		public long SumLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime),
			DateTime end = default(DateTime))
		{
			var sum = _evaluationDataDbController.SumLongs(gameId, actorId, key, start, end);

			Logger.Debug($"Sum: {sum} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return sum;
		}

		public float GetHighestFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var highest = _evaluationDataDbController.GetHighestFloat(gameId, actorId, key, start, end);

			Logger.Debug(
				$"Highest: {highest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return highest;
		}

		public long GetHighestLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var highest = _evaluationDataDbController.GetHighestLong(gameId, actorId, key, start, end);

			Logger.Debug(
				$"Highest: {highest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return highest;
		}


		public EvaluationData GetEvaluationDataByHighestFloat(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var highest = _evaluationDataDbController.GetEvaluationDataByHighestFloat(gameId, actorId, key, start, end);

			Logger.Debug(
				highest != null
					? $"HighestId: {highest.Id} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}"
					: $"Highest not found for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return highest;
		}

		public EvaluationData GetEvaluationDataByHighestLong(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var highest = _evaluationDataDbController.GetEvaluationDataByHighestLong(gameId, actorId, key, start, end);

			Logger.Debug(
				highest != null
					? $"HighestId: {highest.Id} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}"
					: $"Highest not found for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return highest;
		}

		public float GetLowestFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var lowest = _evaluationDataDbController.GetLowestFloat(gameId, actorId, key, start, end);

			Logger.Debug(
				$"Lowest: {lowest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return lowest;
		}

		public long GetLowestLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var lowest = _evaluationDataDbController.GetLowestLong(gameId, actorId, key, start, end);

			Logger.Debug(
				$"Lowest: {lowest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return lowest;
		}

		public bool TryGetLatestLong(int? gameId, int? actorId, string key, out long latest,
			DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var didGetLatest = _evaluationDataDbController.TryGetLatestLong(gameId, actorId, key, out latest, start, end);

			Logger.Debug(
				$"Latest {latest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return didGetLatest;
		}

		public bool TryGetLatestFloat(int? gameId, int? actorId, string key, out float latest,
			DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var didGetLatest = _evaluationDataDbController.TryGetLatestFloat(gameId, actorId, key, out latest, start, end);

			Logger.Debug(
				$"Latest: {latest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return didGetLatest;
		}

		public bool TryGetLatestBool(int? gameId, int? actorId, string key, out bool latest,
			DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var didGetLatest = _evaluationDataDbController.TryGetLatestBool(gameId, actorId, key, out latest, start, end);

			Logger.Debug(
				$"Latest: {latest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return didGetLatest;
		}

		public bool TryGetLatestString(int? gameId, int? actorId, string key, out string latest,
			DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var didGetLatest = _evaluationDataDbController.TryGetLatestString(gameId, actorId, key, out latest, start, end);

			Logger.Debug(
				$"Latest: {latest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return didGetLatest;
		}

		public int CountKeys(int? gameId, int? actorId, string key, EvaluationDataType EvaluationDataType,
			DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var count = _evaluationDataDbController.CountKeys(gameId, actorId, key, EvaluationDataType, start, end);

			Logger.Debug(
				$"Count: {count} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return count;
		}

		// todo change to bool TryGet[name](out value) pattern
		public DateTime TryGetEarliestKey(int? gameId, int? actorId, string key, EvaluationDataType EvaluationDataType,
			DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var didGetEarliestKey = _evaluationDataDbController.TryGetEarliestKey(gameId, actorId, key, EvaluationDataType, start,
				end);

			Logger.Debug(
				$"Earliest: {didGetEarliestKey} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return didGetEarliestKey;
		}

		// todo change to bool TryGet[name](out value) pattern
		public DateTime TryGetLatestKey(int? gameId, int? actorId, string key, EvaluationDataType EvaluationDataType,
			DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var didGetLatestKey = _evaluationDataDbController.TryGetLatestKey(gameId, actorId, key, EvaluationDataType, start, end);

			Logger.Debug(
				$"Earliest: {didGetLatestKey} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

			return didGetLatestKey;
		}

		protected bool ParseCheck(EvaluationData data)
		{
			switch (data.EvaluationDataType)
			{
				case EvaluationDataType.String:
					return true;

				case EvaluationDataType.Long:
					long tryLong;
					return long.TryParse(data.Value, out tryLong);

				case EvaluationDataType.Float:
					float tryFloat;
					return float.TryParse(data.Value, out tryFloat);

				case EvaluationDataType.Boolean:
					bool tryBoolean;
					return bool.TryParse(data.Value, out tryBoolean);

				default:
					return false;
			}
		}

		protected void ValidateData(EvaluationData data)
		{
			if (data.Category != _category)
			{
				throw new InvalidDataException(
					$"Cannot save data with category: {data.Category} with controller for mismatched category: {_category}");
			}

			if (!ParseCheck(data))
			{
				throw new ArgumentException($"Invalid Value {data.Value} for EvaluationDataType {data.EvaluationDataType}");
			}
		}
	}
}