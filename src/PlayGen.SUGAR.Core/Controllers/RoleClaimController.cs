namespace PlayGen.SUGAR.Core.Controllers
{
    public class RoleClaimController
    {
        private readonly Data.EntityFramework.Controllers.RoleClaimController _roleClaimDbController;

        public RoleClaimController(Data.EntityFramework.Controllers.RoleClaimController roleClaimDbController)
        {
            _roleClaimDbController = roleClaimDbController;
        }
    }
}
