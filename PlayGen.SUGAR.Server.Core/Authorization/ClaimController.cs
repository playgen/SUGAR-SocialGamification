using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PlayGen.SUGAR.Common.Permissions;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Authorization
{
    public class ClaimController
    {
        private readonly EntityFramework.Controllers.ClaimController _claimDbController;
        private readonly RoleController _roleDbController;
        private readonly RoleClaimController _roleClaimDbController;

        public ClaimController(EntityFramework.Controllers.ClaimController claimDbController,
                    RoleController roleDbController,
                    RoleClaimController roleClaimDbController)
        {
            _claimDbController = claimDbController;
            _roleDbController = roleDbController;
            _roleClaimDbController = roleClaimDbController;
        }

        public void GetAuthorizationClaims()
        {
            var dbOperations = _claimDbController.Get();
            var currentOperations = new List<Claim>();

            var assembly = Assembly.GetEntryAssembly();
            foreach (var type in assembly.GetTypes())
            {
                var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
                foreach (var method in type.GetMethods(flags))
                {
                    var operation = method.GetCustomAttributes(typeof(AuthorizationAttribute), false) as AuthorizationAttribute[];
                    if (operation != null && operation.Length > 0)
                    {
                        foreach (var op in operation)
                        {
                            if (!currentOperations.Any(co => co.Name == op.Name && co.ClaimScope == op.ClaimScope))
                            {
                                currentOperations.Add(new Claim { ClaimScope = op.ClaimScope, Name = op.Name });
                            }
                        }
                    }
                }
            }
            var newOperations = currentOperations.Where(o => !dbOperations.Any(db => db.Name == o.Name && db.ClaimScope == o.ClaimScope)).ToList();
            newOperations = _claimDbController.Create(newOperations).ToList();

            var roles = _roleDbController.Get().Where(r => r.Default).ToList();

            foreach (var op in newOperations)
            {
                var role = roles.FirstOrDefault(r => r.Name == op.ClaimScope.ToString());
                _roleClaimDbController.Create(new RoleClaim { RoleId = role.Id, ClaimId = op.Id });
            }
        }

        public Claim Get(int id)
        {
            var claim = _claimDbController.Get(id);
            return claim;
        }

        public Claim Get(ClaimScope scope, string name)
        {
            var claim = _claimDbController.Get(scope, name);
            return claim;
        }
    }
}
