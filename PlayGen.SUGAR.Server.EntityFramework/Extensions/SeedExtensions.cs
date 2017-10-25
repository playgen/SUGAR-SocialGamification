using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class SUGARContextSeedExtensions
	{
		public static void Seed(this SUGARContext context)
		{
			var roles = new Dictionary<ClaimScope, Role>();

			foreach (var claimScope in (ClaimScope[])Enum.GetValues(typeof(ClaimScope)))
			{
				var addedClaimScope = context.Roles.Add(new Role
				{
					Name = claimScope.ToString(),
					ClaimScope = claimScope,
					Default = true
				}).Entity;

				roles.Add(claimScope, addedClaimScope);
			}

			var adminUser = context.Users.Add(new User
			{
				Name = "admin"
			}).Entity;

			var adminAccount = context.Accounts.Add(new Account
			{
				Name = "admin",
				Password = "$2a$12$SSIgQE0cQejeH0dM61JV/eScAiHwJo/I3Gg6xZFUc0gmwh0FnMFv.",
				AccountSource = new AccountSource
				{
					Description = "SUGAR",
					Token = "SUGAR",
					RequiresPassword = true
				},
				User = adminUser
			}).Entity;

			#region Actor Roles
			//global (admin)
			context.ActorRoles.Add(new ActorRole
			{
				Role = roles[ClaimScope.Global],
				Actor = adminUser,
				EntityId = Platform.EntityId
			});

			//global game control
			context.ActorRoles.Add(new ActorRole
			{
				Role = roles[ClaimScope.Game],
				Actor = adminUser,
				EntityId = Platform.EntityId
			});

			//global group control
			context.ActorRoles.Add(new ActorRole
			{
				Role = roles[ClaimScope.Group],
				Actor = adminUser,
				EntityId = Platform.EntityId
			});

			// admin user
			context.ActorRoles.Add(new ActorRole
			{
				Role = roles[ClaimScope.User],
				Actor = adminUser,
				EntityId = adminUser.Id
			});

			//global user control
			context.ActorRoles.Add(new ActorRole
			{
				Role = roles[ClaimScope.User],
				Actor = adminUser,
				EntityId = Platform.EntityId
			});

			// admin account
			context.ActorRoles.Add(new ActorRole
			{
				Role = roles[ClaimScope.Account],
				Actor = adminUser,
				EntityId = adminUser.Id
			});

			//global account control
			context.ActorRoles.Add(new ActorRole
			{
				Role = roles[ClaimScope.Account],
				Actor = adminUser,
				EntityId = Platform.EntityId
			});

			//global role control
			context.ActorRoles.Add(new ActorRole
			{
				Role = roles[ClaimScope.Role],
				Actor = adminUser,
				EntityId = Platform.EntityId
			});
			#endregion

			#region Claims
			var claims = new List<Claim>();

			foreach (ClaimScope claimScope in Enum.GetValues(typeof(ClaimScope)))
			{
				foreach (AuthorizationAction authorizationAction in Enum.GetValues(typeof(AuthorizationAction)))
				{
					foreach (AuthorizationEntity authorizationEntity in Enum.GetValues(typeof(AuthorizationEntity)))
					{
						var claim = context.Claims.Add(new Claim
						{
							ClaimScope = claimScope,
							Description = "Auto Generated Claim",
							Name = AuthorizationName.Generate(authorizationAction, authorizationEntity)
						}).Entity;

						claims.Add(claim);
					}
				}
			}
			#endregion

			#region Admin Claims

			claims.ForEach(claim =>
			{
				context.ActorClaims.Add(new ActorClaim
				{
					Actor = adminUser,
					Claim = claim,
					EntityId = Platform.EntityId
				});
			});
			#endregion
			context.SaveChanges();
		}
	}
}
