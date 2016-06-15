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
    public class GroupDbController : DbController
    {
        public GroupDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public Group Create(string name)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.Groups.Any(g => g.Name == name);

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("A group with the name {0} already exists.", name));
                }

                var group = new Group {
                    Name = name
                };
                context.Groups.Add(group);
                context.SaveChanges();

                return group;
            }
        }

        public Group Get(string name)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var group = context.Groups.Single(g => g.Name == name);

                return group;
            }
        }

        public void Delete(int id)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var group = context.Groups.Single(g => g.Id == id);

                context.Groups.Remove(group);
                context.SaveChanges();
            }
        }
    }
}
