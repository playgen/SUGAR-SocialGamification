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

## Roadmap
* Relationship between two groups, creating an [Alliance](/articles/Alliances)