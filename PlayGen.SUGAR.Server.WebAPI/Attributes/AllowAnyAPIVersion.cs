using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SUGAR.Server.WebAPI.Attributes
{
	/// <summary>
	/// Attribute to mark methods that don't require matching API versions.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class AllowAnyAPIVersion : Attribute
	{
	}
}
