using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class RelationshipExtensions
	{
		public static RelationshipResponse ToContract(this UserToGroupRelationship relationshipModel)
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

		public static RelationshipResponse ToContract(this UserToUserRelationship relationshipModel)
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


		public static UserToUserRelationship ToUserModel(this RelationshipRequest relationContract)
		{
			return new UserToUserRelationship {
				RequestorId = relationContract.RequestorId,
				AcceptorId = relationContract.AcceptorId
			};
		}

		public static UserToGroupRelationship ToGroupModel(this RelationshipRequest relationContract)
		{
			return new UserToGroupRelationship {
				RequestorId = relationContract.RequestorId,
				AcceptorId = relationContract.AcceptorId
			};
		}
	}
}