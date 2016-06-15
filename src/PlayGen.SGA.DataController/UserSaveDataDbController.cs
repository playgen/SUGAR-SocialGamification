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
    public class UserSaveDataDbController : DbController
    {
        public UserSaveDataDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public void Create(int id, Contracts.SaveData newData)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                UserData data = new UserData
                {
                    Id = id,
                    GameId = newData.GameId,
                    Key = newData.Key,
                    Value = newData.Value
                };
                
                context.UserDatas.Add(data);
                context.SaveChanges();
            }
        }
    }
}
