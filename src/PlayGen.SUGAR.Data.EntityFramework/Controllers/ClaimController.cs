using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Permissions;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class ClaimController : DbController
	{
		public ClaimController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public List<Claim> Get()
		{
			using (var context = ContextFactory.Create())
			{
				var claims = context.Claims.ToList();
				return claims;
			}
		}

		public Claim Get(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var claim = context.Claims.Find(context, id);
				return claim;
			}
		}

		public Claim Get(ClaimScope scope, string name)
		{
			using (var context = ContextFactory.Create())
			{
				var claim = context.Claims.FirstOrDefault(c => c.ClaimScope == scope && c.Token == name);
				return claim;
			}
		}

		public List<Claim> Create(List<Claim> claims)
		{
			using (var context = ContextFactory.Create())
			{
				var claimList = claims ?? claims.ToList();
				context.Claims.AddRange(claimList);
				SaveChanges(context);

				return claimList;
			}
		}
	}
}