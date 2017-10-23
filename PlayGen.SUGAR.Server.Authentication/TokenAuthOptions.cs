using System;
using Microsoft.IdentityModel.Tokens;

namespace PlayGen.SUGAR.ServerAuthentication
{
    public class TokenAuthOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public TimeSpan ValidityTimeout { get; set; }
    }
}