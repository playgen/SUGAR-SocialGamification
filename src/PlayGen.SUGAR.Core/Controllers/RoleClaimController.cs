using System;
using System.Collections.Generic;

using PlayGen.SUGAR.Data.Model;
using System.Linq;

using PlayGen.SUGAR.Core.Authorization;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class RoleClaimController
    {
        private readonly Data.EntityFramework.Controllers.RoleClaimController _roleClaimDbController;
        private readonly ClaimController _claimDbController;
        private readonly RoleController _roleDbController;
        private readonly Data.EntityFramework.Controllers.ActorRoleController _actorRoleDbController;


        public RoleClaimController(ClaimController claimDbController,
                    RoleController roleDbController,
                    Data.EntityFramework.Controllers.RoleClaimController roleClaimDbController,
                    Data.EntityFramework.Controllers.ActorRoleController actorRoleDbController)
        {
            _claimDbController = claimDbController;
            _roleDbController = roleDbController;
            _roleClaimDbController = roleClaimDbController;
            _actorRoleDbController = actorRoleDbController;
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

        public RoleClaim Create(RoleClaim newRoleClaim, int actorId)
        {
            var roleScope = _roleDbController.GetById(newRoleClaim.RoleId).ClaimScope;
            var claimScope = _claimDbController.Get(newRoleClaim.ClaimId).ClaimScope;
            var roles = _actorRoleDbController.GetActorRoles(actorId);
            var claims = _roleClaimDbController.GetClaimsByRoles(roles.Select(r => r.Id)).Select(c => c.Id);
            if (!claims.Contains(newRoleClaim.ClaimId))
            {
                throw new ArgumentException($"Claim ClaimScope {claimScope} cannot be added");
            }
            if (roleScope != claimScope)
            {
                throw new ArgumentException($"Claim ClaimScope {claimScope} does not match Role ClaimScope {roleScope}");
            }
            newRoleClaim = _roleClaimDbController.Create(newRoleClaim);
            return newRoleClaim;
        }

        public void Delete(int roleID, int claim)
        {
            var role = _roleDbController.GetById(roleID);
            if (role.Name == role.ClaimScope.ToString())
            {
                throw new ArgumentException($"Cannot remove claims from default roles");
            }
            _roleClaimDbController.Delete(roleID, claim);
        }
    }
}
