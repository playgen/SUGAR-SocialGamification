namespace PlayGen.SUGAR.ServerAuthentication
{
	public class PasswordEncryption
	{
		private const int BCryptWorkFactor = 13;

		public static string Encrypt(string password)
		{
		    return BCrypt.Net.BCrypt.HashPassword(password, BCryptWorkFactor);
		}

        public static bool Verify(string password, string hash)
		{
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
	}
}
