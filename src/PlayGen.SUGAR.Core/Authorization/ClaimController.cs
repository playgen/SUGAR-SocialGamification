using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Authorization
{
    public class ClaimController
    {
        private readonly Data.EntityFramework.Controllers.ClaimController _claimDbController;
        private readonly Data.EntityFramework.Controllers.RoleController _roleDbController;
        private readonly Data.EntityFramework.Controllers.RoleClaimController _roleClaimDbController;

        public ClaimController(Data.EntityFramework.Controllers.ClaimController claimDbController,
                    Data.EntityFramework.Controllers.RoleController roleDbController,
                    Data.EntityFramework.Controllers.RoleClaimController roleClaimDbController)
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
                foreach (var method in type.GetMethods())
                {
                    var operation = method.GetCustomAttributes(typeof(AuthorizationAttribute), false).SingleOrDefault() as AuthorizationAttribute;
                    if (operation != null)
                    {
                        if (!currentOperations.Any(co => co.Token == operation.Name && co.ClaimScope == operation.ClaimScope))
                        {
                            currentOperations.Add(new Claim { ClaimScope = operation.ClaimScope, Token = operation.Name });
                        }
                    }
                }
            }
            var newOperations = currentOperations.Where(o => !dbOperations.Any(db => db.Token == o.Token && db.ClaimScope == o.ClaimScope)).ToList();
            newOperations = _claimDbController.Create(newOperations).ToList();

            var roles = _roleDbController.Get().ToList();

            foreach (var op in newOperations)
            {
                var role = roles.FirstOrDefault(r => r.Name == op.ClaimScope.ToString());
                _roleClaimDbController.Create(new RoleClaim { RoleId = role.Id, ClaimId = op.Id });
            }
        }
    }
}
