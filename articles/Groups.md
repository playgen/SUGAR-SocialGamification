# Group
Groups are actors representing collection of actors. They may be individual or multiple game persistent. Groups can be moderated through administrator tools or users, or set up and managed through the game as described by the game designers. 

[Relationships](/articles/Relationships) describe which actors belong to which groups. Groups can have associated achievements,  which can be set for all members of a group to complete. 

Actors can join, leave or add another actor to a group. 


## Features
* CRUD Groups
* CRUD Group Metadata
	* Group Name
	* Group Description
	* Group Icon
* Update Group Membership
* Search Group (ID/name/Actor)



## API
* ClientAPI
    * [GroupClient](/api/PlayGen.SUGAR.ClientAPI.GroupClient)
* Contracts
    * [ActorResponse](/api/PlayGen.SUGAR.Contracts.ActorResponse)
* WebAPI
    * [GroupController](/api/PlayGen.SUGAR.WebAPI.Controllers.GroupController)
    
## Roadmap
* Groups Alliance. 
Provide the ability for relationship between groups. To form an [Alliance](/article/Alliances)

* Group leader.
Providing the ability for individual actors to control the group membership, to add or remove other actors.

* Extended group achievements.
Provide the ability to set the parameters such as number of actors required to meet the achievement criteria before it’s considered complete. 
