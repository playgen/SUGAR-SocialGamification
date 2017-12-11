using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace PlayGen.SUGAR.Server.WebAPI
{
	public partial class Startup
	{
		private void ConfigureAuthentication(IServiceCollection services)
		{
			services
				.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					// Basic settings - signing key to validate with, audience and issuer.
					options.TokenValidationParameters = new TokenValidationParameters
					{
						IssuerSigningKey = key,
						ValidAudience = tokenOptions.Audience,
						ValidIssuer = tokenOptions.Issuer,
						// When receiving a token, check that we've signed it.
						ValidateIssuer = true,
						ValidateIssuerSigningKey = true,
						// When receiving a token, check that it is still valid.
						ValidateLifetime = true,
						// This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
						// when validating the lifetime. As we're creating the tokens locally and validating them on the same 
						// machines which should have synchronised time, this can be set to zero. Where external tokens are
						// used, some leeway here could be useful.
						ClockSkew = TimeSpan.FromMinutes(0)
					};
				}
			);
		}
	}
}
