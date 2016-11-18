using System;
using System.Collections.Generic;

using PlayGen.SUGAR.Common.Shared.Permissions;
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
            var roles = _roleDbController.Get();
            return roles;
        }

        public Role GetByName(string name)
        {
            var role = _roleDbController.Get(name);
            return role;
        }

        public Role GetById(int id)
        {
            var role = _roleDbController.Get(id);
            return role;
        }

        public IEnumerable<Role> GetByScope(ClaimScope scope)
        {
            var roles = _roleDbController.Get(scope);
            return roles;
        }

        public Role Create(Role newRole)
        {
            newRole = _roleDbController.Create(newRole);
            return newRole;
        }

        public void Delete(int id)
        {
            var role = GetById(id);
            if (role.Name == role.ClaimScope.ToString())
            {
                throw new ArgumentException($"Cannot remove default roles");
            }
            _roleDbController.Delete(id);
        }
    }
}
