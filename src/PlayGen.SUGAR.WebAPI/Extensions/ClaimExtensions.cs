using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class ClaimExtensions
	{
		public static ClaimResponse ToContract(this Claim claimModel)
		{
			if (claimModel == null)
			{
				return null;
			}

			return new ClaimResponse {
				Id = claimModel.Id,
				Token = claimModel.Token,
				Description = claimModel.Description,
				ClaimScope = claimModel.ClaimScope
			};
		}

		public static IEnumerable<ClaimResponse> ToContractList(this IEnumerable<Claim> claimModels)
		{
			return claimModels.Select(ToContract).ToList();
		}
	}
}
