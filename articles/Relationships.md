# Relationship
Relationships are connections between two or more entities. For example a user belonging to a group or a user being friends or following another user. Relationships may follow a step by step process depending on the use case, for example in adding a ‘friend’, the relationship is initially stored as a request from the requester to the receiver until accepted by the receiver. Whereas the receiver may also refuse or block the request. 

## Features
* CRUD Relationship 
* CRUD Relationship request
* Search Relationship (ID/Actor)

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