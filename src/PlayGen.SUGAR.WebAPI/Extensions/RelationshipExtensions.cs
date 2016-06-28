using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class RelationshipExtensions
	{
		public static RelationshipResponse ToContract(this UserToGroupRelationship relationshipModel)
		{
			var relationshipContract = new RelationshipResponse
			{
				RequestorId = relationshipModel.RequestorId,
				AcceptorId = relationshipModel.AcceptorId
			};

			return relationshipContract;
		}

		public static RelationshipResponse ToContract(this UserToUserRelationship relationshipModel)
		{
			var relationshipContract = new RelationshipResponse
			{
				RequestorId = relationshipModel.RequestorId,
				AcceptorId = relationshipModel.AcceptorId
			};

			return relationshipContract;
		}

		
		public static UserToUserRelationship ToUserModel(this RelationshipRequest relationContract)
		{
			var relationModel = new UserToUserRelationship
			{
				RequestorId = relationContract.RequestorId,
				AcceptorId = relationContract.AcceptorId
			};

			return relationModel;
		}

		public static UserToGroupRelationship ToGroupModel(this RelationshipRequest relationContract)
		{
			var relationModel = new UserToGroupRelationship
			{
				RequestorId = relationContract.RequestorId,
				AcceptorId = relationContract.AcceptorId
			};

			return relationModel;
		}
	}
}