using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class ActorRoleExtensions
	{
		public static ActorRoleResponse ToContract(this ActorRole actorRoleModel)
		{
			if (actorRoleModel == null)
				return null;

			return new ActorRoleResponse
			{
				Id = actorRoleModel.Id,
				ActorId = actorRoleModel.ActorId,
				RoleId = actorRoleModel.RoleId,
				EntityId = actorRoleModel.EntityId
			};
		}

		public static CollectionResponse ToCollectionContract(this IEnumerable<ActorRole> models)
		{
			return new CollectionResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}

		public static ActorRole ToModel(this ActorRoleRequest actorRoleContract)
		{
			return new ActorRole
			{
				ActorId = actorRoleContract.ActorId,
				RoleId = actorRoleContract.RoleId,
				EntityId = actorRoleContract.EntityId
			};
		}
	}
}