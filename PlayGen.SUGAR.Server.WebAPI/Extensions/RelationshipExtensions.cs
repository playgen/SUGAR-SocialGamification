using System.Diagnostics.CodeAnalysis;

using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class RelationshipExtensions
	{
		public static RelationshipResponse ToContract(this ActorRelationship relationshipModel)
		{
			if (relationshipModel == null)
			{
				return null;
			}

			return new RelationshipResponse {
				RequestorId = relationshipModel.RequestorId,
				AcceptorId = relationshipModel.AcceptorId
			};
		}

		public static ActorRelationship ToRelationshipModel(this RelationshipRequest relationContract)
		{
			return new ActorRelationship
			{
				RequestorId = relationContract.RequestorId.Value,
				AcceptorId = relationContract.AcceptorId.Value
			};
		}
	}
}