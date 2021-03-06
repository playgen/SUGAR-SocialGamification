﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class RoleClaimExtensions
	{
		public static RoleClaimResponse ToContract(this RoleClaim roleClaimModel)
		{
			if (roleClaimModel == null)
			{
				return null;
			}

			return new RoleClaimResponse {
				RoleId = roleClaimModel.RoleId,
				ClaimId = roleClaimModel.ClaimId
			};
		}

		public static IEnumerable<RoleClaimResponse> ToContractList(this IEnumerable<RoleClaim> roleClaimModels)
		{
			return roleClaimModels.Select(ToContract).ToList();
		}

		public static RoleClaim ToModel(this RoleClaimRequest roleClaimContract)
		{
			return new RoleClaim {
				RoleId = roleClaimContract.RoleId.Value,
				ClaimId = roleClaimContract.ClaimId.Value
			};
		}
	}
}
