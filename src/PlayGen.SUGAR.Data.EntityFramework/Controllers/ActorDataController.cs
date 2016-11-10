using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class ActorDataController : DbController
	{
		public ActorDataController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }
    }
}
