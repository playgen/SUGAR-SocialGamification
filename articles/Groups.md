# Groups
Groups are collections of players, which are globally persistant between games. They can be modulated by admin tools or the players themselves, the choice at the discrection of the game designer. [Relationships](/articles/Relationships) describe which players belong to which groups. Group [Achievements](/articles/Achievements) can be set for members of the group to progress towards.

## Features
* Get all groups
* Get all groups that match a given name
* Get a group that matches a given id
* Create a new group
* Delete a group


## API
* ClientAPI
    * [GroupClient](/api/PlayGen.SUGAR.ClientAPI.GroupClient)
* Contracts
    * [ActorResponse](/api/PlayGen.SUGAR.Contracts.ActorResponse)
* WebAPI
    * [GroupController](/api/PlayGen.SUGAR.WebAPI.Controllers.GroupController)
    
## Roadmap
* Groups can friend other groups to form an [Alliance](/article/Alliances)