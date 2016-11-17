using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Core.Controllers;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Authorization
{
    public class ClaimController
    {
        private readonly Data.EntityFramework.Controllers.ClaimController _claimDbController;
        private readonly RoleController _roleDbController;
        private readonly RoleClaimController _roleClaimDbController;

        public ClaimController(Data.EntityFramework.Controllers.ClaimController claimDbController,
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
                foreach (var method in type.GetMethods())
                {
                    var operation = method.GetCustomAttributes(typeof(AuthorizationAttribute), false) as AuthorizationAttribute[];
                    if (operation != null && operation.Length > 0)
                    {
                        foreach (var op in operation)
                        {
                            if (!currentOperations.Any(co => co.Token == op.Name && co.ClaimScope == op.ClaimScope))
                            {
                                currentOperations.Add(new Claim { ClaimScope = op.ClaimScope, Token = op.Name });
                            }
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
                _roleClaimDbController.Create(new RoleClaim { RoleId = role.Id, ClaimId = op.Id }, 0);
            }
        }

        public Claim Get(int id)
        {
            var claim = _claimDbController.Get(id);
            return claim;
        }
    }
}
