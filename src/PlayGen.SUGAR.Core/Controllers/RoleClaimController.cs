using System.Collections.Generic;

using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class RoleClaimController
    {
        private readonly Data.EntityFramework.Controllers.RoleClaimController _roleClaimDbController;

        public RoleClaimController(Data.EntityFramework.Controllers.RoleClaimController roleClaimDbController)
        {
            _roleClaimDbController = roleClaimDbController;
        }

        public IEnumerable<Claim> GetClaimsByRole(int roleId)
        {
            var roles = _roleClaimDbController.GetClaimsByRole(roleId);
            return roles;
        }

        public IEnumerable<Claim> GetClaimsByRoles(IEnumerable<int> ids)
        {
            var roles = _roleClaimDbController.GetClaimsByRoles(ids);
            return roles;
        }

        public RoleClaim Create(RoleClaim newRoleClaim)
        {
            newRoleClaim = _roleClaimDbController.Create(newRoleClaim);
            return newRoleClaim;
        }

        public void Delete(int role, int claim)
        {
            _roleClaimDbController.Delete(role, claim);
        }
    }
}
