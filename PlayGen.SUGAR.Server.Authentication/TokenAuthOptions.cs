using System;
using Microsoft.IdentityModel.Tokens;

namespace PlayGen.SUGAR.Server.Authentication
{
    public class TokenAuthOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public TimeSpan SessionTokenValidityTimeout { get; set; }
        public TimeSpan LoginTokenValidityTimeout { get; set; }
		
	}
}