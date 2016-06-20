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

        public IEnumerable<User> Get()
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var users = context.Users.ToList();

                return users;
            }
        }

        public IEnumerable<User> Get(string[] names)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var users = context.Users.Where(g => names.Contains(g.Name)).ToList();

                return users;
            }
        }

        public IEnumerable<User> Get(int[] id)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var users = context.Users.Where(g => id.Contains(g.Id)).ToList();

                return users;
            }
        }

        public void Create(User user)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                context.Users.Add(user);
                SaveChanges(context);
            }
        }

        public void Delete(int[] id)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var user = context.Users.Where(u => id.Contains(u.Id)).ToList();

                context.Users.RemoveRange(user);
                SaveChanges(context);
            }
        }
    }
}
