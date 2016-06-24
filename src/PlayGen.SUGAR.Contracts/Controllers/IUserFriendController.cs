using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts.Controllers
{
	public interface IUserFriendController
	{
		IEnumerable<ActorResponse> GetFriendRequests(int userId);

		IEnumerable<ActorResponse> GetFriends(int userId);

		RelationshipResponse CreateFriendRequest(RelationshipRequest relationship);

		void UpdateFriendRequest(RelationshipStatusUpdate relationship);

		void UpdateFriend(RelationshipStatusUpdate relationship);
	}
}