# Games
A game is an individual application as defined by the platform. A single instance of the platform has the capacity to service multiple games. This allows for users to take part in multiple of games under the same login and to potentially share resource across games depending on the game design. 

## Features
* CRUD Game
* CRUD Game Metadata
	* Game Name
	* Game Description
	* Game Skills (Skills)	
* Search Game (ID/Name/Actor)

## API
* Client
    * [GameClient](xref:PlayGen.SUGAR.Client.GameClient)
* Contracts
    * [GameRequest](xref:PlayGen.SUGAR.Contracts.GameRequest)
    * [GameResponse](xref:PlayGen.SUGAR.Contracts.GameResponse)
* WebAPI
    * [GameController](xref:PlayGen.SUGAR.WebAPI.Controllers.GameController)
