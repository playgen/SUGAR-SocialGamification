# Relationships
If a player belongs to a group, or a group to an alliance, or a friendship between players; each are described as a relationship. 

## Features
* Get all groups or users which have relationship requests for a specified actor
* Creation
* Updating
* Deletion


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
