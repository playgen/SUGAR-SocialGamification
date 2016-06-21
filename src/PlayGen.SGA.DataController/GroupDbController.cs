using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var groups = context.Groups.ToList();

                return groups;
            }
        }

        public IEnumerable<Group> Get(string[] names)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var groups = context.Groups.Where(g => names.Contains(g.Name)).ToList();

                return groups;
            }
        }

        public void Create(Group group)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                context.Groups.Add(group);
                SaveChanges(context);
            }
        }

        public void Delete(int[] id)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var groups = context.Groups.Where(g => id.Contains(g.Id)).ToList();

                context.Groups.RemoveRange(groups);
                SaveChanges(context);
            }
        }
    }
}
