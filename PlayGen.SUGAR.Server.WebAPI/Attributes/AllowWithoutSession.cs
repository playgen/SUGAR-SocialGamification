using System;

namespace PlayGen.SUGAR.Server.WebAPI.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
    public class AllowWithoutSession : Attribute
    {
    }
}
