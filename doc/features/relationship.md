# Relationship
Relationships are connections between two or more entities. For example a user belonging to a group or a user being friends or following another user. Relationships may follow a step by step process depending on the use case, for example in adding a ‘friend’, the relationship is initially stored as a request from the requester to the receiver until accepted by the receiver. Whereas the receiver may also refuse or block the request. 

## Features
* CRUD Relationship 
* CRUD Relationship request
* Search Relationship (ID/Actor)

## API
* Client
    * [GroupMemberClient](xref:PlayGen.SUGAR.Client.GroupMemberClient)
    * [UserFriendClient](xref:PlayGen.SUGAR.Client.UserFriendClient)
* Contracts
    * [RelationshipStatusUpdate](xref:PlayGen.SUGAR.Contracts.RelationshipStatusUpdate)
    * [RelationshipRequest](xref:PlayGen.SUGAR.Contracts.RelationshipRequest)
    * [RelationshipResponse](xref:PlayGen.SUGAR.Contracts.RelationshipResponse)
    * [ActorResponse](xref:PlayGen.SUGAR.Contracts.ActorResponse)
* WebAPI
    * [GroupMemberController](xref:PlayGen.SUGAR.WebAPI.Controllers.GroupMemberController)
    * [UserFriendController](xref:PlayGen.SUGAR.WebAPI.Controllers.UserFriendController)

## Examples
* Adding a User to a Group
	A [Group](group.md) can be joined by an actor. This will create a user to group relationship request. In this example, we will set the AutoAccept property in the [RelationshipRequest](xref:PlayGen.SUGAR.Contracts.RelationshipRequest) object to true, so the relationship will be stored directly as a user to group relationship. The joined group's id is then extracted from the [RelationshipResponse](xref:PlayGen.SUGAR.Contracts.RelationshipResponse).

```cs
		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private GroupMemberClient _groupMemberClient;
		private int _userId;
		private int _groupId;

		private void JoinGroup(int groupId) 
		{
			// create instance of the group member client
			_groupMemberClient = sugarClient.GroupMember;

			// create a RelationshipRequest
			var relationshipRequest = new RelationshipRequest 
			{
				AcceptorId = groupId,
				RequestorId = _userId,
				AutoAccept = true
			};

			// create the member request and store the response
			var relationshipResponse = _groupMemberClient.CreateMemberRequest(relationshipRequest);

			// store the id of the group for use in other functions
			_groupId = relationshipResponse.AcceptorId;
		}
```

* Leaving a group
	A user to group relationship status can be updated using a [RelationshipStatusUpdate](xref:PlayGen.SUGAR.Contracts.RelationshipStatusUpdate) with the [GroupMemberClient](xref:PlayGen.SUGAR.Client.GroupMemberClient)'s UpdateMember function. This example shows the user leaving the group joined in the previous example. Calling the function automatically breaks the relationship between the group and player if there is one, without the need of passing the additional Accepted property in the RelationshipStatusUpdate.

```cs
		private void LeaveGroup() 
		{
			// create a RelationshipStatusUpdate
			var relationshipStatusUpdate = new RelationshipStatusUpdate 
			{
				AcceptorId = _groupId,
				RequestorId = _userId
			};

			// create the member request and store the response
			_groupMemberClient.UpdateMember(relationshipStatusUpdate);
		}
```


* Adding a friend
	Works identically to joining a group, except creating user to user relationships and using the [UserFriendClient](xref:PlayGen.SUGAR.Client.UserFriendClient). 

```cs
		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private UserFriendClient _userFriendClient;
		private int _userId;
		private int _friendId;

		private void JoinGroup(int targetUserId) 
		{
			// create instance of the user friend client
			_userFriendClient = sugarClient.UserFriend;

			// create a RelationshipRequest
			var relationshipRequest = new RelationshipRequest 
			{
				AcceptorId = targetUserId,
				RequestorId = _userId,
				AutoAccept = true
			};

			// create the friend request and store the response
			var relationshipResponse = _userFriendClient.CreateFriendRequest(relationshipRequest);

			// store the id of the group for use in other functions
			_friendId = relationshipResponse.AcceptorId;
		}
```

* Removing a friend
	Like leaving a group, removing a friend updates the user to user relationship using a [RelationshipStatusUpdate](xref:PlayGen.SUGAR.Contracts.RelationshipStatusUpdate) with [UserFriendClient](xref:PlayGen.SUGAR.Client.UserFriendClient)'s UpdateFriend function. 

```cs
		private void RemoveFriend() 
		{
			// create a RelationshipStatusUpdate
			var relationshipStatusUpdate = new RelationshipStatusUpdate 
			{
				AcceptorId = _friendId,
				RequestorId = _userId,
				Accepted = true
			};

			// create the member request and store the response
			_userFriendClient.UpdateFriend(relationshipStatusUpdate);
		}
```

## Roadmap
* Relationship between two groups, creating an [Alliance](/articles/Alliances)