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


## Examples
* Create a game
	
	Creating a game using the [GameClient](xref:PlayGen.SUGAR.Client.GameClient)'s Create function, passing a [GameRequest](xref:PlayGen.SUGAR.Contracts.GameRequest) object as the parameter. This example will be used to create a game with the name "Thrones" and store its Id from the returned [GameResponse](xref:PlayGen.SUGAR.Contracts.GameResponse).

```cs

		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private GameClient _gameClient;
		private int _gameId;

		private void CreateGame() 
		{
			// create instance of the game client
			_gameClient = sugarClient.Game;

			// create a GameRequest
			var gameRequest = new GameRequest 
			{
				Name = "Thrones"
			};

			// create the game and store the response
			var gameResponse = _gameClient.Create(gameRequest);

			// store the id of the game for use in other features
			_gameId = gameResponse.Id;
		}

```

* Retreiving a game

	Checking if a Game exists or storing the id of the Game prior to allowing the user to play may be vital. This is done using [GameClient](xref:PlayGen.SUGAR.Client.GameClient)'s Get function and passing the name of the game to match.

```cs 

	private int CheckGame() 
	{
		// check for the game and store the responses
		var gameResponses = _gameClient.Get("Thrones");

		int id = -1;

		foreach (response in gameResponses) 
		{
			// check if the name matches the desired game exactly
			if (response.Name == "Thrones") 
			{	
				// store the game's id
				id = response.Id;
			}
		}

		return id;
	}

```