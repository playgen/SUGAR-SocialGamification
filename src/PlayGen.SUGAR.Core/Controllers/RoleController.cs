using System.Collections.Generic;

using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class RoleController
    {
        private readonly Data.EntityFramework.Controllers.RoleController _roleDbController;

        public RoleController(Data.EntityFramework.Controllers.RoleController roleDbController)
        {
            _roleDbController = roleDbController;
        }

        public IEnumerable<Role> Get()
        {
            var games = _roleDbController.Get();
            return games;
        }

        public Role Create(Role newRole)
        {
            newRole = _roleDbController.Create(newRole);
            return newRole;
        }

        public void Delete(int id)
        {
            _roleDbController.Delete(id);
        }
    }
}
