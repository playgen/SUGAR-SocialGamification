using System;

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
