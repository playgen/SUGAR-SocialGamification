---
uid: resource
---

# Resources
Resources provide a flexible set of game objects which may be associated with actors. Resources may represent or track such objects as scores, experience points, in-game currency or in-game items. They may be earned, spent, gifted or otherwise associated. Resource provide the ability for inventories to be assigned to individual or group actors. 

Resources are game objects which are obtained and exchanged by players. They may be consumable or permanent. Examples of resources include in-game currency, items, gifts and tools. A resource can be set to exist outside a game instance, allowing exchanges from external social platforms. Modulation of resources is handled by the <xref:evaluationData> system. Resources can belong to a group, where it becomes shared by all members of that group. 

## Features
* CRUD Resources
* Search Resources (ID/Name/Actor/Relationship) 
* Gift resource from one actor to another


## API
* Client
    * [ResourceClient](xref:PlayGen.SUGAR.Client.ResourceClient)
* Contracts
	* [ResourceAddRequest](xref:PlayGen.SUGAR.Contracts.ResourceAddRequest)
	* [ResourceResponse](xref:PlayGen.SUGAR.Contracts.ResourceResponse)
	* [ResourceTransferRequest](xref:PlayGen.SUGAR.Contracts.ResourceTransferRequest)
	* [ResourceTransferResponse](xref:PlayGen.SUGAR.Contracts.ResourceTransferResponse)
* WebAPI
    * [ResourceController](xref:PlayGen.SUGAR.Server.WebAPI.Controllers.ResourceController)

## Examples
* Creating a resource
	The [ResourceClient](xref:PlayGen.SUGAR.Client.ResourceClient) has an AddOrUpdate function which adds a new resource entry belonging to the user into [GameData](gameData.md) or automatically updates an existing one if a duplicate entry was to be made. The function takes a [ResourceAddRequest](xref:PlayGen.SUGAR.Contracts.ResourceAddRequest) parameter and returns a [ResourceResponse](xref:PlayGen.SUGAR.Contracts.ResourceResponse). This example will show how to both add a new entry or increment a player's amount of Valyrian steel. This is done by specifying the key "ValyrianSteel" which will be added with a value matching the quantity to GameData. If the entry already exists, it will add the quantity to its value (or subtract if the quantity is negative).

```cs
		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private ResourceClient _resourceClient;
		private int _gameId;
		private int _userId;

		private void AddOrUpdateResource(int quantity) 
		{
			// create instance of the resource client
			_resourceClient = sugarClient.Resource;

			// create a ResourceAddRequest
			var resourceAddRequest = new ResourceAddRequest 
			{
				GameId = _gameId,
				ActorId = _userId,
				Key = "ValyrianSteel",
				Quantity = quantity
			};

			// Add the resource or update an existing key
			_resourceClient.AddOrUpdate(resourceAddRequest);
		}
```

* Transfer a resource
	This example will show how to give Valryian to another player. The [ResourceClient](xref:PlayGen.SUGAR.Client.ResourceClient)'s Transfer function handles this taking a [ResourceTransferRequest](xref:PlayGen.SUGAR.Contracts.ResourceTransferRequest) parameter and returning a [ResourceTransferResponse](xref:PlayGen.SUGAR.Contracts.ResourceTransferResponse) object. When the transfer is made, it adds the resource quantity to the target user, and subtracts it from the source user. 

```cs 
		private void TransferResource(int quantity, int targetUser) 
		{
			// create a ResourceTransferRequest
			var resourceTransferRequest = new ResourceTransferRequest 
			{
				GameId = _gameId,
				SenderActorId = _userId,
				RecipientActorId = targetUser,
				Key = "ValyrianSteel",
				Quantity = quantity,
			};

			// transfer the resources from the user to the target
			_resourceClient.Transfer(resourceTransferRequest);
		}
```

## Roadmap
* Read/write access management for group resources

* Extended permissions.
Proving mechanism to set ownership, and control of usage access. For example a player may own an item in the game which they can ‘lend’ to another player to use for a period, without the other player owning it. 

*Extended metadata.
Providing mechanism to record additional metadata against resources such as being able to rate them or track a history of owners or uses. 

*Tradable resources
Providing mechanism for actors to trade and exchange resource, including management of agreement by multiple parties through escrow system. 

