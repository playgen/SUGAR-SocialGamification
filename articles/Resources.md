# Resources
Resources are game objects which are obtained and exchanged by players. They may be consumable or permanent. Examples of resources include in-game currency, items, gifts and tools. A resource can be set to exist outside a game instance, allowing exchanges from external social platforms. Modulation of resources is handled by the [GameData](/articles/GameData) system. Resources can belong to a group, where it becomes shared by all members of that group. 

## Features
* Get resources belonging to a group or player
* Gift a resource from one actor to another
* Trade a number of resources between actors
* Create a new resource
* Delete a resource


## API
* ClientAPI
    * [ResourceClient](/api/PlayGen.SUGAR.ClientAPI.ResourceClient)
* Contracts

* WebAPI
    * [ResourceController](/api/PlayGen.SUGAR.WebAPI.Controllers.ResourceController)


## Roadmap
* Read/write access management for group resources