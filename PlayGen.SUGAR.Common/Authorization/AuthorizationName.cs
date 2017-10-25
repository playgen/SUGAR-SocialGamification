using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Common.Authorization
{
    public class AuthorizationName
    {
		public static string Generate(AuthorizationAction action, AuthorizationEntity entity)
		{
			return $"{action}-{entity}";
		}
    }
}
