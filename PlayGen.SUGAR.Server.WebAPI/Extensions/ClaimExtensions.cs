using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
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
				Token = claimModel.Name,
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
