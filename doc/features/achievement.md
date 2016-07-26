---
uid: achievement
---

# Achievements
Achievements provide a flexible and simple way to increase user engagement, tracking their actions within and across multiple games. Achievements may be viewed as goals that individual or multiple actors can acquire through meeting one or more criteria. 

The criteria is flexible and can be written by the game developer or provided by the platform based on any game specific action. Achievements can be binary or incremental, ie. (complete/not complete) or have levels or percentages of progression toward their completion. They can be global, across all games, or associated with a single game. 

## Features
* Add/Update/Delete achievements
* Search for achievements by Id/Game/Name/Actor

## API
* Client
    * <xref:PlayGen.SUGAR.Client.AchievementClient>
* Contracts
    * <xref:PlayGen.SUGAR.Contracts.AchievementCriteria>
    * <xref:PlayGen.SUGAR.Contracts.AchievementProgressResponse>
    * <xref:PlayGen.SUGAR.Contracts.AchievementRequest>
    * <xref:PlayGen.SUGAR.Contracts.AchievementResponse>
    * <xref:PlayGen.SUGAR.Contracts.GameDataType>
    * <xref:PlayGen.SUGAR.Contracts.CriteriaQueryType>
    * <xref:PlayGen.SUGAR.Contracts.ComparisonType>
    * <xref:PlayGen.SUGAR.Contracts.ActorType>
    * <xref:PlayGen.SUGAR.Contracts.CriteriaScope>
* WebAPI
    * <xref:PlayGen.SUGAR.WebAPI.Controllers.AchievementsController>

## Examples
* Specifying an achievement

	This example will describe how to implement the 'Slay 10 Enemies!' achievement. An achievement must be specified with an @PlayGen.SUGAR.Contracts.AchievementRequest) with a list of @PlayGen.SUGAR.Contracts.AchievementCriteria) that will be evalutated against to determine progress.
	All 'EnemiesSlain' keys will be checked in gameData and whether the sum of their values is greater than or equal to 10.

```cs
		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private AchievementClient _achievementClient;
		private int _gameId;
		private int _userId;

		private void SetUpAchievement()
		{
			// create instance of the achievement client
			_achievementClient = sugarClient.Achievement;
			
			// create an AchievementCriteria list
			var achievementCriteria = new List<AchievementCriteria>()
			{
				new AchievementCriteria()
				{
					DataType = GameDataType.Long,
					Value = "10",
					Key = "EnemiesSlain",
					CriteriaQueryType = CriteriaQueryType.Sum,
					ComparisonType = ComparisonType.GreaterOrEqual,
					Scope = CriteriaScope.Actor
				}
			};
			
			// place the criteria inside an AchievementRequest
			var achievementRequest = new AchievementRequest()
			{
				GameId = _gameId,
				Name = "Slay 10 Enemies!",
				ActorType = ActorType.User,
				Token = "slay_10_enemies",
				CompletionCriteria = achievementCriteria
			};

			// create the achievement
			_achievementClient.Create(achievementRequest);
		}
```

* Submitting data for when somthing which may be used to evaluate progress towards an Achievement  
	
	An achievement uses keys in <xref:gameData> that match its <xref:PlayGen.SUGAR.Contracts.AchievementCriteria>. This data is submitted at points in the game which demonstrate progress towards the achievement (as well as other uses).

```cs
		private void SlayEnemies(int quantity)
		{
			// *Enemy Slaying Code* //


			// create instance of GameDataClient
			var gameDataClient = sugarClient.GameData;

			// create GameDataRequest
			var gameDataRequest = new GameDataRequest()
			{
				GameId = _gameId,
				ActorId = _userId,
				GameDataType = GameDataType.Long,
				Value = quantity,
				Key = "EnemiesSlain"
			};

			// add the GameData
			gameDataClient.Add(gameDataRequest);
		}
```

* Checking an achiement's progress

	Using the <xref:PlayGen.SUGAR.Client.AchievementClient> and specifying the GameId, ActorId and Token, returns an <xref:PlayGen.SUGAR.Contracts.AchievementProgressResponse> object for that Actor's progress towards the achievement in that game. 

```cs
		private float CheckAchievementProgress()
		{
			// Check the user's progress towards the achievements in the specified game
			var achievementProgressResponse = _achievementClient.GetAchievementProgress
			(
				"slay_10_enemies", 
				_gameId,
				 _userId
			 );

			// Output the progress
			return achivementProgressResponse.Progress;
		}
```

## Roadmap

* Portable achievement system.
Many game networks such as the Google play services, Apple gamkits, Microsoft game centre, Facebook game services and Steam Works offer achievements. Integrating multiple achievement systems into a game is time consuming. Additionally platform providers regularly update their APIs, creating a head-ache for keeping up-to-date with multiple systems. The achievement system will provide the ability for the actors’ achievement to be recorded automatically against other 3rd party systems.   

* Challenge system.
Achievement system will be extended by adding temporal (time based) and ownership component. This extension will be referred to as Challenge.  Challenges may be seen as achievements that can be suggested to actors, gifted by one actor to another, accepted or  rejected by an actor, as well as tracked to see if actors attempted or abandoned them. 

