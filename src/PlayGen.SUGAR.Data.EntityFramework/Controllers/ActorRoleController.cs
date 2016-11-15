using System.Collections.Generic;
using System.Linq;

using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
    public class ActorRoleController : DbController
    {
        public ActorRoleController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        public IEnumerable<Role> GetActorRolesForEntity(int actorId, int entityId)
        {
            using (var context = ContextFactory.Create())
            {
                var roles = context.ActorRoles.Where(ar => ar.ActorId == actorId && ar.EntityId == entityId).Select(ar => ar.Role).ToList();
                return roles;
            }
        }

        public IEnumerable<ActorRole> GetActorRoles(int actorId)
        {
            using (var context = ContextFactory.Create())
            {
                var roles = context.ActorRoles.Where(ar => ar.ActorId == actorId).ToList();
                return roles;
            }
        }

        public IEnumerable<Actor> GetRoleActors(int roleId, int entityId)
        {
            using (var context = ContextFactory.Create())
            {
                var actors = context.ActorRoles.Where(ar => ar.RoleId == roleId && ar.EntityId == entityId).Select(ar => ar.Actor).ToList();
                return actors;
            }
        }

        public ActorRole Create(ActorRole actorRole)
        {
            using (var context = ContextFactory.Create())
            {
                context.ActorRoles.Add(actorRole);
                SaveChanges(context);

                return actorRole;
            }
        }

        public void Delete(int id)
        {
            using (var context = ContextFactory.Create())
            {
                var actorRole = context.ActorRoles
                    .Where(r => id == r.Id);

                context.ActorRoles.RemoveRange(actorRole);
                SaveChanges(context);
            }
        }
    }
}
