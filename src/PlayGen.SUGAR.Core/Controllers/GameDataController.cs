using PlayGen.SUGAR.Data.Model;
using NLog;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.EntityFramework;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class GameDataController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly SaveDataController _saveDataController;

        public GameDataController(SUGARContextFactory contextFactory)
        {
            _saveDataController = new SaveDataController(contextFactory, SaveDataCategory.GameData);
        }

        public List<SaveData> Get(int? gameId, int? actorId, string[] key)
        {
            return _saveDataController.Get(gameId, actorId, key);
        }

        public SaveData Add(SaveData newData)
        {
            return _saveDataController.Add(newData);
        }

        public void Add(SaveData[] newDatas)
        {
            _saveDataController.Add(newDatas);
        }
    }
}