using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.DataController
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class DbController
    {
        private readonly string _nameOrConnectionString;

        public DbController(string nameOrConnectionString)
        {
            _nameOrConnectionString = nameOrConnectionString;
        }

        #region Create
        public int CreateUser(string name)
        {
            int id = -1;

            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);
                context.Database.CreateIfNotExists();
                // TODO: enforce user with same name doesn't already exist

                var user = new User(name);
                context.Users.Add(user);
                context.SaveChanges();

                id = user.Id;
            }
            
            return id;
        }

        public int CreateGroup(string name)
        {
            return -1;
        }
        #endregion

        void SetLog(DbContext context)
        {
            context.Database.Log = Console.WriteLine;
        }
    }
}
