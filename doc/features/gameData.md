# GameData
GameData is a storage system, following a key-value structure. It is used by SUGAR features and provides flexibility in providing custom game-specific storage solutions.

GameData provides storage for [Achievements](/articles/Achievements), [Skills](/articles/Skills), [Resources](/articles/Resources) and custom general data. 

GameData storage is used by [Achievements](/articles/Achievements), [Leaderboards](/articles/Leaderboards), [Skills](/articles/Skills), [Resources](/articles/Resources) and custom general data. 

## Features
* CRD GameData 
* Search GameData (by ID/Game/Actor/Skill)

## API
* Client
    * [GameDataClient](xref:PlayGen.SUGAR.Client.GameDataClient)
* Contracts
    * [GameDataRequest](xref:PlayGen.SUGAR.Contracts.GameDataRequest)
    * [GameDataResponse](xref:PlayGen.SUGAR.Contracts.GameDataResponse)
* WebAPI
    * [GameDataController](xref:PlayGen.SUGAR.WebAPI.Controllers.GameDataController)

## Examples
* Submitting custom GameData

 	Custom GameData is submitted using the [GameDataClient](xref:PlayGen.SUGAR.Client.GameDataClient)'s Add function with a [GameDataRequest](xref:PlayGen.SUGAR.Contracts.GameDataRequest) as the parameter. This explains how to track the number of dragon eggs hatched by the user, specifying "EggHatched" as the key.

```cs 
		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private GameDataClient _gameDataClient;
		private int _gameId;
		private int _userId;

		private void OnEggHatched()
		{
			// create instance of GameDataClient
			var gameDataClient = sugarClient.GameData;

			// create GameDataRequest
			var gameDataRequest = new GameDataRequest()
			{
				GameId = _gameId,
				ActorId = _userId,
				GameDataType = GameDataType.Long,
				Value = "1",
				Key = "EggHatched"
			};

			// add the GameData
			gameDataClient.Add(gameDataRequest);
		}
```

* Querying for GameData

	GameData is retreived using the [GameDataClient](xref:PlayGen.SUGAR.Client.GameDataClient)'s Get function with the ActorId and GameId you want to query as parameters. The parameters also takes a list of keys you want to find entries for in gameData. This example shows how to retrieve the user's "EggHatched" [GameDataResponse](xref:PlayGen.SUGAR.Contracts.GameDataResponse) objects and count them.

```cs 
	
	private long GetEggsHatched()
	{
		// add the GameData
		var gameDataResponses = gameDataClient.Get
		(
			_actorId,
			_gameId,
			new string[] { "EggHatched" }
		);

		long totalClicks = 0;

		// count values from each GameDataResponse
		foreach (var response in gameDataResponses)
		{
			totalClicks += long.Parse(response.Value);
		}

		return totalClicks;
	}

```