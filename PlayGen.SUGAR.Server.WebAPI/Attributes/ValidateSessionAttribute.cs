using System;

namespace PlayGen.SUGAR.Server.WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateSessionAttribute : Attribute
    {
    }
}
