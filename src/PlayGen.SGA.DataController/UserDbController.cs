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

        public User Create(User newUser)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.Users.Any(u => u.Name == newUser.Name);

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("A user with the name {0} already exists.", newUser.Name));
                }

                var user = newUser;
                context.Users.Add(user);
                context.SaveChanges();

                return user;
            }
        }

        public IEnumerable<User> Get(string[] names)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var users = context.Users.Where(g => names.Contains(g.Name)).ToList();

                return users;
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
