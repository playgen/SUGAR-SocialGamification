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

        public void Create(int id, Contracts.SaveData newData)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                GroupData data = new GroupData
                {
                    Id = id,
                    GameId = newData.GameId,
                    Key = newData.Key,
                    Value = newData.Value
                };
                context.GroupDatas.Add(data);
                context.SaveChanges();
            }
        }
    }
}