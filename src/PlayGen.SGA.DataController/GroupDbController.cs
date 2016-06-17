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

        public IEnumerable<Group> Get()
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var groups = context.Groups.ToList();

                return groups;
            }
        }

        public IEnumerable<Group> Get(string[] names)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var groups = context.Groups.Where(g => names.Contains(g.Name)).ToList();

                return groups;
            }
        }

        public Group Create(Group newGroup)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.Groups.Any(g => g.Name == newGroup.Name);

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("A group with the name {0} already exists.", newGroup.Name));
                }

                var group = newGroup;
                context.Groups.Add(group);
                context.SaveChanges();

                return group;
            }
        }

        public void Delete(int[] id)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var groups = context.Groups.Where(g => id.Contains(g.Id)).ToList();

                context.Groups.RemoveRange(groups);
                context.SaveChanges();
            }
        }
    }
}
