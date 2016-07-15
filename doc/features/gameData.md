# GameData
GameData is a storage system, following a key-value structure. It is used by SUGAR features and provides flexibility in providing custom game-specific storage solutions.

GameData provides storage for [Achievements](features/achievement.md), [Skills](features/skill.md), [Resources](features/resource.md) and custom general data. 

GameData storage is used by [Achievements](features/achievement.md), [Leaderboards](features/leaderboard.md), [Skills](features/skill.md), [Resources](features/resource.md) and custom general data. 

## Features
* CRD GameData 
* Search GameData (by ID/Game/Actor/Skill)

## GameData Categories
- General Data - All information that does not fit into any other category.
    - Example - A [user](/features/user.html) gaining 10 points in level 2.
- Resources - Long-only data which can be transferred to and from [actors](/features/actor.html) within the system.
    - Example - A user collecting 10 gold, a consumable item within the game. 
- Achievements - Stored completion of all [AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria) for an achievement.
    - Example - A user meeting the criteria for the achievement "Score 10,000 points" in a game.
- Skills - Stored completion of all [AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria) for an skill.
    - Example - A user meeting the criteria for the "Social" skill in a game.

## API
* Client
    * [GameDataClient](xref:PlayGen.SUGAR.Client.GameDataClient)
* Contracts
    * [GameDataRequest](xref:PlayGen.SUGAR.Contracts.GameDataRequest)
    * [GameDataResponse](xref:PlayGen.SUGAR.Contracts.GameDataResponse)
    * [GameDataType](xref:PlayGen.SUGAR.Contracts.CriteriaQueryType)
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