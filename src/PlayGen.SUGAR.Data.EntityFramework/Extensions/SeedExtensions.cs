using System;

using PlayGen.SUGAR.Common.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
	// ReSharper disable once InconsistentNaming
	internal static class SUGARContextSeedExtensions
	{
		internal static void Seed(this SUGARContext context)
		{
			foreach (var claimScope in (ClaimScope[])Enum.GetValues(typeof(ClaimScope)))
			{
				context.Roles.Add(new Role
				{
					Name = claimScope.ToString(),
					ClaimScope = claimScope,
					Default = true
				});
			}

			context.Accounts.Add(new Account()
			{
				Name = "admin",
				Password = "$2a$12$SSIgQE0cQejeH0dM61JV/eScAiHwJo/I3Gg6xZFUc0gmwh0FnMFv.",
				Id = 1,
				AccountSource = new AccountSource
				{
					Description = "SUGAR",
					Token = "SUGAR",
					RequiresPassword = true
				},
				AccountSourceId = 1,
				User = new User
				{
					Id = 1,
					Name = "admin"
				},
				UserId = 1
			});

			//global (admin)
			context.ActorRoles.Add(new ActorRole
			{
				RoleId = (int)ClaimScope.Global + 1,
				ActorId = 1,
				EntityId = -1
			});

			//global game control
			context.ActorRoles.Add(new ActorRole
			{
				RoleId = (int)ClaimScope.Game + 1,
				ActorId = 1,
				EntityId = -1
			});

			//global group control
			context.ActorRoles.Add(new ActorRole
			{
				RoleId = (int)ClaimScope.Group + 1,
				ActorId = 1,
				EntityId = -1
			});

			//user
			context.ActorRoles.Add(new ActorRole
			{
				RoleId = (int)ClaimScope.User + 1,
				ActorId = 1,
				EntityId = 1
			});

			//global user control
			context.ActorRoles.Add(new ActorRole
			{
				RoleId = (int)ClaimScope.User + 1,
				ActorId = 1,
				EntityId = -1
			});

			//account
			context.ActorRoles.Add(new ActorRole
			{
				RoleId = (int)ClaimScope.Account + 1,
				ActorId = 1,
				EntityId = 1
			});

			//global account control
			context.ActorRoles.Add(new ActorRole
			{
				RoleId = (int)ClaimScope.Account + 1,
				ActorId = 1,
				EntityId = -1
			});

			//global role control
			context.ActorRoles.Add(new ActorRole
			{
				RoleId = (int)ClaimScope.Role + 1,
				ActorId = 1,
				EntityId = -1
			});

			context.SaveChanges();
		}
	}
}
