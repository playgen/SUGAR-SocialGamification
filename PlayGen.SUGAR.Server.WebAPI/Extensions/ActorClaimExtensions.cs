using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class ActorClaimExtensions
	{
		public static ActorClaimResponse ToContract(this ActorClaim actorClaimModel)
		{
			if (actorClaimModel == null)
			{
				return null;
			}

			return new ActorClaimResponse
			{
				Id = actorClaimModel.Id,
				ActorId = actorClaimModel.ActorId,
				ClaimId = actorClaimModel.Claim.Id,
				ClaimName = actorClaimModel.Claim.Name,
				EntityId = actorClaimModel.EntityId
			};
		}

		public static IEnumerable<ActorClaimResponse> ToContractList(this IEnumerable<ActorClaim> actorClaimModels)
		{
			return actorClaimModels.Select(ToContract).ToList();
		}

		public static ActorClaim ToModel(this ActorClaimRequest actorClaimContract)
		{
			return new ActorClaim
			{
				ActorId = actorClaimContract.ActorId.Value,
				ClaimId = actorClaimContract.ClaimId.Value,
				EntityId = actorClaimContract.EntityId.Value
			};
		}

	}
}