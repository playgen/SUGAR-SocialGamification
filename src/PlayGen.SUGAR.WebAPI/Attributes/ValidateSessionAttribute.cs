using System;

namespace PlayGen.SUGAR.WebAPI.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ValidateSessionAttribute : Attribute
	{
	}
}