using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class ClaimExtensions
	{
		public static ClaimResponse ToContract(this Claim claimModel)
		{
			if (claimModel == null)
				return null;

			return new ClaimResponse
			{
				Id = claimModel.Id,
				Token = claimModel.Token,
				Description = claimModel.Description,
				ClaimScope = claimModel.ClaimScope
			};
		}

		public static CollectionResponse ToCollectionContract(this IEnumerable<Claim> models)
		{
			return new CollectionResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}
	}
}