using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class GroupController
    {
        private readonly Data.EntityFramework.Controllers.GroupController _groupDbController;

        public GroupController(Data.EntityFramework.Controllers.GroupController groupDbController)
        {
            _groupDbController = groupDbController;
        }
        
        public IEnumerable<Group> Get()
        {
            var groups = _groupDbController.Get();
            return groups;
        }

        public Group Get(int id)
        {
            var group = _groupDbController.Get(id);
            return group;
        }

        public IEnumerable<Group> Search(string name)
        {
            var groups = _groupDbController.Get(name);
            return groups;
        }
        
        public Group Create(Group newGroup)
        {
            newGroup = _groupDbController.Create(newGroup);
            return newGroup;
        }
        
        public void Update(Group group)
        {
            _groupDbController.Update(group);
        }
        
        public void Delete(int id)
        {
            _groupDbController.Delete(id);
        }
    }
}