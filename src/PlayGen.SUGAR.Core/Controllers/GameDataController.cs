using System;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class GameDataController
    {
        public static Action<GameData> GameDataAddedEvent;

        private readonly Data.EntityFramework.Controllers.GameDataController _gameDataDbController;

        public GameDataController(Data.EntityFramework.Controllers.GameDataController gameDataDbController)
        {
            _gameDataDbController = gameDataDbController;
        }
        
        public IEnumerable<GameData> Get(int? gameId, int? actorId, string[] key)
        {
            var datas = _gameDataDbController.Get(gameId, actorId, key);
            return datas;
        }

        public void Add(GameData[] datas)
        {
            var dataList = new List<GameData>();
            var i = 0;
            var chunkSize = 1000;

            var uniqueAddedData = new Dictionary<string, GameData>();

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
                    _gameDataDbController.Create(dataList);
                    dataList.Clear();
                }

                i++;

            } while (dataList.Count > 0 && i < datas.Length);

            foreach (var addedData in uniqueAddedData.Values)
            {
                GameDataAddedEvent?.Invoke(addedData);   
            }
        }

        public GameData Add(GameData newData)
        {
            if (ParseCheck(newData))
            {
                newData = _gameDataDbController.Create(newData);

                GameDataAddedEvent?.Invoke(newData);

                return newData;
            }
            else
            {
                throw new ArgumentException($"Invalid Value {newData.Value} for SaveDataType {newData.SaveDataType}");
            }
        }

        public bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var keyExists = _gameDataDbController.KeyExists(gameId, actorId, key, start, end);
            return keyExists;
        }

        public IEnumerable<GameData> Get(IEnumerable<int> ids)
        {
            var datas = _gameDataDbController.Get(ids);
            return datas;
        }

        public IEnumerable<GameData> Get(int? gameId = null, int? actorId = null, IEnumerable<string> keys = null)
        {
            var datas = _gameDataDbController.Get(gameId, actorId, keys);
            return datas;
        }

        public IEnumerable<long> AllLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var longs = _gameDataDbController.AllLongs(gameId, actorId, key, start, end);
            return longs;
        }

        public IEnumerable<float> AllFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var floats = _gameDataDbController.AllFloats(gameId, actorId, key, start, end);
            return floats;
        }

        public IEnumerable<string> AllStrings(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var strings = _gameDataDbController.AllStrings(gameId, actorId, key, start, end);
            return strings;
        }

        public IEnumerable<bool> AllBools(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var bools = _gameDataDbController.AllBools(gameId, actorId, key, start, end);
            return bools;
        }

        public float SumFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var sum = _gameDataDbController.SumFloats(gameId, actorId, key, start, end);
            return sum;
        }

        public long SumLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var sum = _gameDataDbController.SumLongs(gameId, actorId, key, start, end);
            return sum;
        }

        public float GetHighestFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var highest = _gameDataDbController.GetHighestFloats(gameId, actorId, key, start, end);
            return highest;
        }

        public long GetHighestLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var highest = _gameDataDbController.GetHighestLongs(gameId, actorId, key, start, end);
            return highest;
        }

        public float GetLowestFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var lowest = _gameDataDbController.GetLowestFloats(gameId, actorId, key, start, end);
            return lowest;
        }

        public long GetLowestLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var lowest = _gameDataDbController.GetLowestLongs(gameId, actorId, key, start, end);
            return lowest;
        }

        public bool TryGetLatestLong(int? gameId, int? actorId, string key, out long latest, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetLatest = _gameDataDbController.TryGetLatestLong(gameId, actorId, key, out latest, start, end);
            return didGetLatest;
        }

        public bool TryGetLatestFloat(int? gameId, int? actorId, string key, out float latest, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetLatest = _gameDataDbController.TryGetLatestFloat(gameId, actorId, key, out latest, start, end);
            return didGetLatest;
        }

        public bool TryGetLatestBool(int? gameId, int? actorId, string key, out bool latest, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetLatest = _gameDataDbController.TryGetLatestBool(gameId, actorId, key, out latest, start, end);
            return didGetLatest;
        }

        public bool TryGetLatestString(int? gameId, int? actorId, string key, out string latest, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetLatest = _gameDataDbController.TryGetLatestString(gameId, actorId, key, out latest, start, end);
            return didGetLatest;
        }

        public int CountKeys(int? gameId, int? actorId, string key, SaveDataType SaveDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var count = _gameDataDbController.CountKeys(gameId, actorId, key, SaveDataType, start, end);
            return count;
        }

        // todo change to bool TryGet[name](out value) pattern
        public DateTime TryGetEarliestKey(int? gameId, int? actorId, string key, SaveDataType SaveDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetEarliestKey = _gameDataDbController.TryGetEarliestKey(gameId, actorId, key, SaveDataType, start, end);
            return didGetEarliestKey;
        }

        // todo change to bool TryGet[name](out value) pattern
        public DateTime TryGetLatestKey(int? gameId, int? actorId, string key, SaveDataType SaveDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            var didGetLatestKey = _gameDataDbController.TryGetLatestKey(gameId, actorId, key, SaveDataType, start, end);
            return didGetLatestKey;
        }

        protected bool ParseCheck(GameData data)
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