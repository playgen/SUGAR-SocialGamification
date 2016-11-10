using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
    public class PermissionController : DbController
    {
        public PermissionController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }
    }
}
