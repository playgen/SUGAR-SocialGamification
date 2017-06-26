using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class RoleClaimExtensions
	{
		public static RoleClaimResponse ToContract(this RoleClaim roleClaimModel)
		{
			if (roleClaimModel == null)
				return null;

			return new RoleClaimResponse
			{
				RoleId = roleClaimModel.RoleId,
				ClaimId = roleClaimModel.ClaimId
			};
		}

		public static CollectionResponse ToCollectionContract(this IEnumerable<RoleClaim> models)
		{
			return new CollectionResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}

		public static RoleClaim ToModel(this RoleClaimRequest roleClaimContract)
		{
			return new RoleClaim
			{
				RoleId = roleClaimContract.RoleId,
				ClaimId = roleClaimContract.ClaimId
			};
		}
	}
}