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
