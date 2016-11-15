using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
    public class RoleClaimController : DbController
    {
        public RoleClaimController(SUGARContextFactory contextFactory) 
			: base(contextFactory)
		{
        }

        public IEnumerable<Claim> GetByRoles(IEnumerable<int> ids)
        {
            using (var context = ContextFactory.Create())
            {
                var claims = context.RoleClaims.Where(rc => ids.Contains(rc.RoleId)).Select(rc => rc.Claim).ToList();
                claims = claims.Distinct().ToList();
                return claims;
            }
        }
    }
}
