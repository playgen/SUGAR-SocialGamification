using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SUGAR.Server.WebAPI.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
    public class AllowWithoutSession : Attribute
    {
    }
}
