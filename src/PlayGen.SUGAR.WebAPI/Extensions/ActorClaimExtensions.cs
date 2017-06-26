using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class ActorClaimExtensions
	{
		public static ActorClaimResponse ToContract(this ActorClaim actorClaimModel)
		{
			if (actorClaimModel == null)
				return null;

			return new ActorClaimResponse
			{
				Id = actorClaimModel.Id,
				ActorId = actorClaimModel.ActorId,
				ClaimId = actorClaimModel.Claim.Id,
				ClaimName = actorClaimModel.Claim.Token,
				EntityId = actorClaimModel.EntityId
			};
		}

		public static CollectionResponse ToCollectionContract(this IEnumerable<ActorClaim> models)
		{
			return new CollectionResponse() {
				Items = models.Select(ToContract).ToArray(),
			};

		}

		public static ActorClaim ToModel(this ActorClaimRequest actorClaimContract)
		{
			return new ActorClaim
			{
				ActorId = actorClaimContract.ActorId,
				ClaimId = actorClaimContract.ClaimId,
				EntityId = actorClaimContract.EntityId
			};
		}
	}
}