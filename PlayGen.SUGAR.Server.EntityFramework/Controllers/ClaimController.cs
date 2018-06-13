using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
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
				var claim = context.Claims.Find(id);
				return claim;
			}
		}

		public Claim Get(ClaimScope scope, string name)
		{
			using (var context = ContextFactory.Create())
			{
				var claim = context.Claims.FirstOrDefault(c => c.ClaimScope == scope && c.Name == name);
				return claim;
			}
		}

		public List<Claim> Create(List<Claim> claims)
		{
			using (var context = ContextFactory.Create())
			{
				var claimList = claims ?? new List<Claim>();
				context.Claims.AddRange(claimList);
				context.SaveChanges();

				return claimList;
			}
		}
	}
}
