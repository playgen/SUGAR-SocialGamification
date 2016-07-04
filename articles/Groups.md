# Groups
Groups are collections of players. They can be modulated by admin tools or the players themselves, the choice at the discrection of the developer. The [Relationship](/articles/Relationships) between a player and group is handled by [GroupMemberController](/api/PlayGen.SUGAR.WebAPI.Controllers.GroupMemberController) and associated ClientAPI. Group [Achievements](/articles/Achievements) can be set for members of the group to progress towards and work identically to regular achievements.

## Features
* Get all groups
* Get all groups that match a given name
* Get a group that matches a given id
* Creation
* Deletion


## API
* ClientAPI
    * [GroupClient](/api/PlayGen.SUGAR.ClientAPI.GroupClient)
* Contracts
    * [ActorResponse](/api/PlayGen.SUGAR.Contracts.ActorResponse)
* WebAPI
    * [GroupController](/api/PlayGen.SUGAR.WebAPI.Controllers.GroupController)
    
