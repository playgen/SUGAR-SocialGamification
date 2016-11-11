using System;

namespace PlayGen.SUGAR.Common.Shared.Permissions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthOperation : Attribute
    {
        public ClaimScope ClaimScope { get; set; }

        public string Name { get; set; }

        public AuthOperation(ClaimScope scope, string name)
        {
            ClaimScope = scope;
            Name = name;
        }

        public const string Create = "Create";
        public const string Read = "Read";
        public const string Update = "Update";
        public const string Delete = "Delete";
    }
}
