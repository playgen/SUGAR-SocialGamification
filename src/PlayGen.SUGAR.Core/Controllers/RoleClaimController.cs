using System;
using System.Collections.Generic;

using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class RoleClaimController
    {
        private readonly Data.EntityFramework.Controllers.RoleClaimController _roleClaimDbController;
        private readonly Data.EntityFramework.Controllers.ClaimController _claimDbController;
        private readonly Data.EntityFramework.Controllers.RoleController _roleDbController;

        public RoleClaimController(Data.EntityFramework.Controllers.ClaimController claimDbController,
                    Data.EntityFramework.Controllers.RoleController roleDbController,
                    Data.EntityFramework.Controllers.RoleClaimController roleClaimDbController)
        {
            _claimDbController = claimDbController;
            _roleDbController = roleDbController;
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
            var roleScope = _roleDbController.Get(newRoleClaim.RoleId).ClaimScope;
            var claimScope = _claimDbController.Get(newRoleClaim.ClaimId).ClaimScope;
            if (roleScope != claimScope)
            {
                throw new ArgumentException($"Claim ClaimScope {claimScope} does not match Role ClaimScope {roleScope}");
            }
            newRoleClaim = _roleClaimDbController.Create(newRoleClaim);
            return newRoleClaim;
        }

        public void Delete(int role, int claim)
        {
            _roleClaimDbController.Delete(role, claim);
        }
    }
}
