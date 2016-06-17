using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.DataController
{
    public class GroupSaveDataDbController : DbController
    {
        public GroupSaveDataDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public GroupData Create(GroupData newData)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var actorExists = context.Groups.Any(u => u.Id == newData.GroupId);
                var gameExists = context.Games.Any(g => g.Id == newData.GameId);

                if (!actorExists)
                {
                    throw new DuplicateRecordException(string.Format("The provided group does not exist."));
                }

                if (!gameExists)
                {
                    throw new DuplicateRecordException(string.Format("The provided game does not exist."));
                }

                GroupData data = newData;
                context.GroupDatas.Add(data);
                context.SaveChanges();
                return data;
            }
        }

        public IEnumerable<GroupData> Get(int gameId, int groupId, string[] keys)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var data = context.GroupDatas.Where(d => d.GroupId == groupId && d.GameId == gameId && keys.Contains(d.Key)).ToList();

                return data;
            }
        }
    }
}