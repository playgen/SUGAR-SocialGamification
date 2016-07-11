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
* ClientAPI
    * [GameClient](/api/PlayGen.SUGAR.ClientAPI.GameClient)
* Contracts
    * [GameRequest](/api/PlayGen.SUGAR.Contracts.GameRequest)
    * [GameResponse](/api/PlayGen.SUGAR.Contracts.GameResponse)
* WebAPI
    * [GameController](/api/PlayGen.SUGAR.WebAPI.Controllers.GameController)
