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
    public class UserDbController : DbController
    {
        public UserDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public User Create(string name)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.Users.Any(u => u.Name == name);

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("A user with the name {0} already exists.", name));
                }

                var user = new User
                {
                    Name = name
                };
                context.Users.Add(user);
                context.SaveChanges();

                return user;
            }
        }

        public User Get(string name)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var user = context.Users.Single(u => u.Name == name);

                return user;
            }
        }

        public void Delete(int id)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var user = context.Users.Single(u => u.Id == id);

                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
    }
}
