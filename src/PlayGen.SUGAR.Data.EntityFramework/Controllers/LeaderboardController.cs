using System.Linq;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class LeaderboardController : DbController
	{
		public LeaderboardController(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}
	}
}