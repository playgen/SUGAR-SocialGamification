using System;

using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
    internal static class SUGARContextSeedExtensions
    {
        internal static void Seed(this SUGARContext context)
        {
            foreach (var claimScope in (ClaimScope[])Enum.GetValues(typeof(ClaimScope)))
            {
                context.Roles.Add(new Role
                {
                    Name = claimScope.ToString(),
                    ClaimScope = claimScope
                });
            }

            context.Accounts.Add(new Account()
            {
                Name = "admin",
                Password = "$2a$12$SSIgQE0cQejeH0dM61JV/eScAiHwJo/I3Gg6xZFUc0gmwh0FnMFv.",
                Id = 1,
                User = new User()
                {
                    Id = 1,
                    Name = "admin"
                },
                UserId = 1
            });

            context.ActorRoles.Add(new ActorRole
            {
                RoleId = 1,
                ActorId = 1,
                EntityId = 0
            });

            context.ActorRoles.Add(new ActorRole
            {
                RoleId = 3,
                ActorId = 1,
                EntityId = 1
            });

            context.ActorRoles.Add(new ActorRole
            {
                RoleId = 4,
                ActorId = 1,
                EntityId = 1
            });

            context.SaveChanges();
        }
    }
}
