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

        public IEnumerable<GroupData> Get(int gameId, int groupId, string[] keys)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var data = context.GroupDatas.Where(d => d.GroupId == groupId && d.GameId == gameId && keys.Contains(d.Key)).ToList();

                return data;
            }
        }

        public GroupData Create(GroupData data)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var actorExists = context.Groups.Any(u => u.Id == data.GroupId);
                var gameExists = context.Games.Any(g => g.Id == data.GameId);

                if (!actorExists)
                {
                    throw new DuplicateRecordException(string.Format("The provided group does not exist."));
                }

                if (!gameExists)
                {
                    throw new DuplicateRecordException(string.Format("The provided game does not exist."));
                }

                context.GroupDatas.Add(data);
                SaveChanges(context);
                return data;
            }
        }
    }
}