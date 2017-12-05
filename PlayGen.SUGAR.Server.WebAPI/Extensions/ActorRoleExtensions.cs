using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class ActorRoleExtensions
	{
		public static ActorRoleResponse ToContract(this ActorRole actorRoleModel)
		{
			if (actorRoleModel == null)
			{
				return null;
			}

			return new ActorRoleResponse {
				Id = actorRoleModel.Id,
				ActorId = actorRoleModel.ActorId,
				RoleId = actorRoleModel.RoleId,
				EntityId = actorRoleModel.EntityId
			};
		}

		public static IEnumerable<ActorRoleResponse> ToContractList(this IEnumerable<ActorRole> actorRoleModels)
		{
			return actorRoleModels.Select(ToContract).ToList();
		}

		public static ActorRole ToModel(this ActorRoleRequest actorRoleContract)
		{
			return new ActorRole {
				ActorId = actorRoleContract.ActorId.Value,
				RoleId = actorRoleContract.RoleId.Value,
				EntityId = actorRoleContract.EntityId.Value
			};
		}

	}
}