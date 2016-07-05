# Relationships
Relationship is the connection held between two or more entities. 
If a player belongs to a group or a friendship between two players; each of these are described as a relationship. A new relationship is first stored in a requests table until it is accepted by the receiver of the request. Once moved into the requests table, the relationship is used to check for group membership or friendships. 

## Features
* Get all groups or users which have relationship requests for a specified actor
* Create a new relationship request
* Update a relationship's status from pending to accepted
* Delete a relationship

## API
* ClientAPI
    * [GroupMemberClient](/api/PlayGen.SUGAR.ClientAPI.GroupMemberClient)
    * [UserFriendClient](/api/PlayGen.SUGAR.ClientAPI.UserFriendClient)
* Contracts
    * [RelationshipStatusUpdate](/api/PlayGen.SUGAR.Contracts.RelationshipStatusUpdate)
    * [RelationshipRequest](/api/PlayGen.SUGAR.Contracts.RelationshipRequest)
    * [RelationshipResponse](/api/PlayGen.SUGAR.Contracts.RelationshipResponse)
    * [ActorResponse](/api/PlayGen.SUGAR.Contracts.ActorResponse)
* WebAPI
    * [GroupMemberController](/api/PlayGen.SUGAR.WebAPI.Controllers.GroupMemberController)
    * [UserFriendController](/api/PlayGen.SUGAR.WebAPI.Controllers.UserFriendController)

## Roadmap
* Relationship between two groups, creating an [Alliance](/articles/Alliances)