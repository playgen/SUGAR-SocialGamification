using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.ServerAuthentication;

namespace PlayGen.SUGAR.WebAPI.Helpers
{
    public static class AuthHelper
    {
        public static AuthRequirement GetAuth(Type callingType, [CallerMemberName] string memberName = "")
        {
            var method = callingType.GetMethods().SingleOrDefault(m => m.Name.Equals(memberName));

            if (method != null)
            {
                var operation = method.GetCustomAttributes(typeof(AuthOperation), false).SingleOrDefault() as AuthOperation;
                if (operation != null)
                {
                    return new AuthRequirement(operation);
                }
            }
            return null;
        }
    }
}
