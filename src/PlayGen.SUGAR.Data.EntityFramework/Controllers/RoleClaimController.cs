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

        public IEnumerable<Claim> GetClaimsByRole(int id)
        {
            using (var context = ContextFactory.Create())
            {
                var claims = context.RoleClaims.Where(rc => id == rc.RoleId).Select(rc => rc.Claim).ToList();
                return claims;
            }
        }

        public IEnumerable<Claim> GetClaimsByRoles(IEnumerable<int> ids)
        {
            using (var context = ContextFactory.Create())
            {
                var claims = context.RoleClaims.Where(rc => ids.Contains(rc.RoleId)).Select(rc => rc.Claim).ToList();
                claims = claims.Distinct().ToList();
                return claims;
            }
        }

        public RoleClaim Create(RoleClaim roleClaim)
        {
            using (var context = ContextFactory.Create())
            {
                context.RoleClaims.Add(roleClaim);
                SaveChanges(context);

                return roleClaim;
            }
        }

        public void Delete(int role, int claim)
        {
            using (var context = ContextFactory.Create())
            {
                var roleClaim = context.RoleClaims
                    .Where(r => role == r.RoleId && claim == r.ClaimId);

                context.RoleClaims.RemoveRange(roleClaim);
                SaveChanges(context);
            }
        }
    }
}
