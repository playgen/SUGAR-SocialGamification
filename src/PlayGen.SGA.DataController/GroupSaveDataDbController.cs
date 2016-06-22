using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataController.Interfaces;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.DataController
{
    public class GroupSaveDataDbController : DbController, ISaveDataDbController
    {
        public GroupSaveDataDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public IEnumerable<GroupData> Get(int gameId, int groupId, string[] keys)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var data = context.GroupDatas.Where(d => d.GroupId == groupId && d.GameId == gameId && keys.Contains(d.Key)).ToList();

                return data;
            }
        }

        public void Create(GroupData data)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var actorExists = context.Groups.Any(u => u.Id == data.GroupId);
                var gameExists = context.Games.Any(g => g.Id == data.GameId);

                if (!actorExists)
                {
                    throw new MissingRecordException(string.Format("The provided group does not exist."));
                }

                if (!gameExists)
                {
                    throw new MissingRecordException(string.Format("The provided game does not exist."));
                }

                context.GroupDatas.Add(data);
                SaveChanges(context);
            }
        }

        // TODO implement based on the code in UserSaveDataController
        public float SumFloats(int gameId, int actorId, string key)
        {
            throw new NotImplementedException();
        }

        // TODO implement based on the code in UserSaveDataController
        public long SumLongs(int gameId, int actorId, string key)
        {
            throw new NotImplementedException();
        }

        // TODO implement based on the code in UserSaveDataController
        public bool TryGetLatestBool(int gameId, int actorId, string key, out bool latestBool)
        {
            throw new NotImplementedException();
        }

        // TODO implement based on the code in UserSaveDataController
        public bool TryGetLatestString(int gameId, int actorId, string key, out string latestString)
        {
            throw new NotImplementedException();
        }
    }
}