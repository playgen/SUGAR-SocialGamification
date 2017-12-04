using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class RoleExtensions
	{
		public static RoleResponse ToContract(this Role roleModel)
		{
			if (roleModel == null)
			{
				return null;
			}

			return new RoleResponse {
				Id = roleModel.Id,
				Name = roleModel.Name,
				Default = roleModel.Default,
				ClaimScope = roleModel.ClaimScope
			};
		}

		public static IEnumerable<RoleResponse> ToContractList(this IEnumerable<Role> roleModels)
		{
			return roleModels.Select(ToContract).ToList();
		}

		public static Role ToModel(this RoleRequest roleContract)
		{
			return new Role {
				Name = roleContract.Name,
				ClaimScope = roleContract.ClaimScope.Value,
				Default = false
			};
		}

	}
}