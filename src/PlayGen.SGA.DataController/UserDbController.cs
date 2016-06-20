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

        public User Create(User user)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.Users.Any(u => u.Name == user.Name);

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("A user with the name {0} already exists.", user.Name));
                }

                context.Users.Add(user);
                context.SaveChanges();

                return user;
            }
        }

        public void Delete(int[] id)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var user = context.Users.Where(u => id.Contains(u.Id)).ToList();

                context.Users.RemoveRange(user);
                context.SaveChanges();
            }
        }
    }
}
