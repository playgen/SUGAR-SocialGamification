using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
    public class ActorClaimController : DbController
    {
        public ActorClaimController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        public bool Check(int actorId, int entityId, int claimId)
        {
            using (var context = ContextFactory.Create())
            {
                var claims = context.ActorClaims.Any(c => c.ActorId == actorId && c.EntityId == entityId && c.ClaimId == claimId);
                return claims;
            }
        }
    }
}
