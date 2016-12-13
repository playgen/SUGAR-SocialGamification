using System;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;
using NLog;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.EntityFramework;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class SaveDataController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static Action<SaveData> SaveDataAddedEvent;

        private readonly Data.EntityFramework.Controllers.SaveDataController _saveDataDbController;

        [Obsolete]
        public SaveDataController(Data.EntityFramework.Controllers.SaveDataController saveDataDbController)
        {
            _saveDataDbController = saveDataDbController;
        }

        public SaveDataController(SUGARContextFactory contextFactory, SaveDataCategory category)
        {
            _saveDataDbController = new Data.EntityFramework.Controllers.SaveDataController(contextFactory, category);
        }

        public void Add(SaveData[] datas)
        {
            var dataList = new List<SaveData>();
            var i = 0;
            var chunkSize = 1000;

            var uniqueAddedData = new Dictionary<string, SaveData>();

            do
            {
                var newData = datas[i];

                if (ParseCheck(newData))
                {
                    dataList.Add(newData);
                    uniqueAddedData[$"{newData.GameId}_{newData.Key}_{newData.Category}"] = newData;
                }
                else
                {
                    throw new ArgumentException($"Invalid Value {newData.Value} for SaveDataType {newData.SaveDataType}");
                }

                if (dataList.Count >= chunkSize || i == datas.Length - 1)
                {
                    _saveDataDbController.Create(dataList);
                    dataList.Clear();
                }

                i++;

            } while (dataList.Count > 0 && i < datas.Length);

            foreach (var addedData in uniqueAddedData.Values)
            {
                SaveDataAddedEvent?.Invoke(addedData);   
            }

            Logger.Info($"Added: {datas?.Length} Game Datas.");
        }

        public SaveData Add(SaveData newData)
        {
            if (ParseCheck(newData))
            {
                newData = _saveDataDbController.Create(newData);

                Logger.Info($"{newData?.Id}");

                SaveDataAddedEvent?.Invoke(newData);

                return newData;
            }
            else
            {
                throw new ArgumentException($"Invalid Value {newData.Value} for SaveDataType {newData.SaveDataType}");
            }
        }

        public bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var keyExists = _saveDataDbController.KeyExists(gameId, actorId, key, start, end);

            Logger.Debug($"Key Exists: {keyExists} for GameId: {gameId}, ActorId: {actorId}, Key: {key}, Start: {start}, End: {end}");

            return keyExists;
        }

        public List<SaveData> Get(List<int> ids)
        {
            var datas = _saveDataDbController.Get(ids);
            
            Logger.Info($"{datas.Count} Game Datas for Ids: {string.Join(", ", ids)}");

            return datas;
        }

        public List<SaveData> Get(int? gameId = null, int? actorId = null, string[] keys = null)
        {
            var datas = _saveDataDbController.Get(gameId, actorId, keys);

            Logger.Info($"{datas?.Count} Game Datas for GameId: {gameId}, ActorId {actorId}, Keys: {string.Join(", ", keys)}");

            return datas;
        }

        public List<long> AllLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var longs = _saveDataDbController.AllLongs(gameId, actorId, key, start, end);

            Logger.Debug($"{longs?.Count} Values for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return longs;
        }

        public List<float> AllFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var floats = _saveDataDbController.AllFloats(gameId, actorId, key, start, end);

            Logger.Debug($"{floats?.Count} Values for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return floats;
        }

        public List<string> AllStrings(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var strings = _saveDataDbController.AllStrings(gameId, actorId, key, start, end);

            Logger.Debug($"{strings?.Count} Values for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return strings;
        }

        public List<bool> AllBools(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var bools = _saveDataDbController.AllBools(gameId, actorId, key, start, end);

            Logger.Debug($"{bools?.Count} Values for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return bools;
        }

        public float SumFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var sum = _saveDataDbController.SumFloats(gameId, actorId, key, start, end);

            Logger.Debug($"Sum: {sum} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return sum;
        }

        public long SumLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var sum = _saveDataDbController.SumLongs(gameId, actorId, key, start, end);

            Logger.Debug($"Sum: {sum} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return sum;
        }

        public float GetHighestFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var highest = _saveDataDbController.GetHighestFloats(gameId, actorId, key, start, end);

            Logger.Debug($"Highest: {highest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return highest;
        }

        public long GetHighestLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var highest = _saveDataDbController.GetHighestLongs(gameId, actorId, key, start, end);

            Logger.Debug($"Highest: {highest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return highest;
        }

        public float GetLowestFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var lowest = _saveDataDbController.GetLowestFloats(gameId, actorId, key, start, end);

            Logger.Debug($"Lowest: {lowest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return lowest;
        }

        public long GetLowestLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var lowest = _saveDataDbController.GetLowestLongs(gameId, actorId, key, start, end);

            Logger.Debug($"Lowest: {lowest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return lowest;
        }

        public bool TryGetLatestLong(int? gameId, int? actorId, string key, out long latest, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetLatest = _saveDataDbController.TryGetLatestLong(gameId, actorId, key, out latest, start, end);

            Logger.Debug($"Latest {latest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return didGetLatest;
        }

        public bool TryGetLatestFloat(int? gameId, int? actorId, string key, out float latest, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetLatest = _saveDataDbController.TryGetLatestFloat(gameId, actorId, key, out latest, start, end);

            Logger.Debug($"Latest: {latest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return didGetLatest;
        }

        public bool TryGetLatestBool(int? gameId, int? actorId, string key, out bool latest, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetLatest = _saveDataDbController.TryGetLatestBool(gameId, actorId, key, out latest, start, end);

            Logger.Debug($"Latest: {latest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return didGetLatest;
        }

        public bool TryGetLatestString(int? gameId, int? actorId, string key, out string latest, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetLatest = _saveDataDbController.TryGetLatestString(gameId, actorId, key, out latest, start, end);

            Logger.Debug($"Latest: {latest} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return didGetLatest;
        }

        public int CountKeys(int? gameId, int? actorId, string key, SaveDataType SaveDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var count = _saveDataDbController.CountKeys(gameId, actorId, key, SaveDataType, start, end);

            Logger.Debug($"Count: {count} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return count;
        }

        // todo change to bool TryGet[name](out value) pattern
        public DateTime TryGetEarliestKey(int? gameId, int? actorId, string key, SaveDataType SaveDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetEarliestKey = _saveDataDbController.TryGetEarliestKey(gameId, actorId, key, SaveDataType, start, end);

            Logger.Debug($"Earliest: {didGetEarliestKey} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return didGetEarliestKey;
        }

        // todo change to bool TryGet[name](out value) pattern
        public DateTime TryGetLatestKey(int? gameId, int? actorId, string key, SaveDataType SaveDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetLatestKey = _saveDataDbController.TryGetLatestKey(gameId, actorId, key, SaveDataType, start, end);

            Logger.Debug($"Earliest: {didGetLatestKey} for: GameId: {gameId}, ActorId {actorId}, Key: {key}, Start: {start}, End: {end}");

            return didGetLatestKey;
        }

        protected bool ParseCheck(SaveData data)
        {
            switch (data.SaveDataType)
            {
                case SaveDataType.String:
                    return true;

                case SaveDataType.Long:
                    long tryLong;
                    return long.TryParse(data.Value, out tryLong);
                    
                case SaveDataType.Float:
                    float tryFloat;
                    return float.TryParse(data.Value, out tryFloat);
                    
                case SaveDataType.Boolean:
                    bool tryBoolean;
                    return bool.TryParse(data.Value, out tryBoolean);

                default:
                    return false;
            }
        }
    }
}