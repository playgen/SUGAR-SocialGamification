---
uid: gameData
---

# GameData

GameData provides a method of recording and querying key value pairs of data specific to a certain <xref:game> and optional <xref:actor>.

## Features
* CRUD Resources
* Search Resources (ID/Name/Actor/Relationship) 
* Gift resource from one actor to another

## Features
* CRD GameData 
* Search GameData (by ID/Game/Actor/Skill)

## GameData Categories
- General Data - All information that does not fit into any other category.
    - Example - A <xref:user> gaining 10 points in level 2.
- Resources - Long-only data which can be transferred to and from <xref:actor> within the system.
    - Example - A user collecting 10 gold, a consumable item within the game. 
- Achievements - Stored completion of all <xref:PlayGen.SUGAR.Common.Shared.EvaluationCriteria> for an achievement.
    - Example - A user meeting the criteria for the achievement "Score 10,000 points" in a game.
- Skills - Stored completion of all <xref:PlayGen.SUGAR.Common.Shared.EvaluationCriteria> for an skill.
    - Example - A user meeting the criteria for the "Social" skill in a game.

## API
* Client
    * <xref:PlayGen.SUGAR.Client.GameDataClient>
* Contracts
    * <xref:PlayGen.SUGAR.Contracts.Shared.EvaluationDataRequest>
    * <xref:PlayGen.SUGAR.Contracts.Shared.EvaluationDataResponse>
    * <xref:PlayGen.SUGAR.Common.Shared.CriteriaQueryType>

## Examples
* Submitting custom GameData

 	Custom GameData is submitted using the <xref:PlayGen.SUGAR.Client.GameDataClient>'s Add function with a <xref:PlayGen.SUGAR.Contracts.Shared.EvaluationDataRequest> as the parameter. This explains how to track the number of dragon eggs hatched by the user, specifying "EggHatched" as the key.

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

	GameData is retreived using the <xref:PlayGen.SUGAR.Client.GameDataClient>'s Get function with the ActorId and GameId you want to query as parameters. The parameters also takes a list of keys you want to find entries for in gameData. This example shows how to retrieve the user's "EggHatched" <xref:PlayGen.SUGAR.Contracts.Shared.EvaluationDataResponse> objects and count them.

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