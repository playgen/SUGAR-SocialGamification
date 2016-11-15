using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Authorization
{
    public class ClaimController
    {
        private readonly Data.EntityFramework.Controllers.ClaimController _claimDbController;

        public ClaimController(Data.EntityFramework.Controllers.ClaimController claimDbController)
        {
            _claimDbController = claimDbController;
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
                        if (!currentOperations.Any(co => co.Token == operation.Name && co.PermissionType == operation.ClaimScope))
                        {
                            currentOperations.Add(new Claim { PermissionType = operation.ClaimScope, Token = operation.Name });
                        }
                    }
                }
            }
            var newOperations = currentOperations.Where(o => !dbOperations.Any(db => db.Token == o.Token && db.PermissionType == o.PermissionType)).ToList();
            _claimDbController.Create(newOperations);
        }
    }
}
