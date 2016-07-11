# Resources
Resources provide a flexible set of game objects which may be associated with actors. Resources may represent or track such objects as scores, experience points, in-game currency or in-game items. They may be earned, spent, gifted or otherwise associated. Resource provide the ability for inventories to be assigned to individual or group actors. 

Resources are game objects which are obtained and exchanged by players. They may be consumable or permanent. Examples of resources include in-game currency, items, gifts and tools. A resource can be set to exist outside a game instance, allowing exchanges from external social platforms. Modulation of resources is handled by the [GameData](/articles/GameData) system. Resources can belong to a group, where it becomes shared by all members of that group. 

## Features
* CRUD Resources
* Search Resources (ID/Name/Actor/Relationship) 
* Gift resource from one actor to another


## API
* ClientAPI
    * [ResourceClient](/api/PlayGen.SUGAR.ClientAPI.ResourceClient)
* Contracts

* WebAPI
    * [ResourceController](/api/PlayGen.SUGAR.WebAPI.Controllers.ResourceController)


## Roadmap
* Read/write access management for group resources

* Extended permissions.
Proving mechanism to set ownership, and control of usage access. For example a player may own an item in the game which they can ‘lend’ to another player to use for a period, without the other player owning it. 

*Extended metadata.
Providing mechanism to record additional metadata against resources such as being able to rate them or track a history of owners or uses. 

*Tradable resources
Providing mechanism for actors to trade and exchange resource, including management of agreement by multiple parties through escrow system. 

