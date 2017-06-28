using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class RoleExtensions
	{
		public static RoleResponse ToContract(this Role roleModel)
		{
			if (roleModel == null)
				return null;

			return new RoleResponse
			{
				Id = roleModel.Id,
				Name = roleModel.Name,
				Default = roleModel.Default,
				ClaimScope = roleModel.ClaimScope
			};
		}

		public static RolesResponse ToCollectionContract(this IEnumerable<Role> models)
		{
			return new RolesResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}

		public static Role ToModel(this RoleRequest roleContract)
		{
			return new Role
			{
				Name = roleContract.Name,
				ClaimScope = roleContract.ClaimScope,
				Default = false
			};
		}
	}
}