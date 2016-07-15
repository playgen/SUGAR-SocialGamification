# Leaderboard
Leaderboards provide a visual comparison tool for measurements of resources or objects associated with actors. A wide variety of leaderboards are supported including those based achievement or resource based on individual, group or single and multiple games. Leaderboards can utilise any [GameData](gameData.md), [Actor](actor.md),[Skill](skill.md) or [Resource](resource.md). 

## Features
* Generate a leaderboard by:
	* Game / Resource / Skill / Actor
	* ActorType (Group, Player)
	* Key (from GameData, Resource, Actor, Skill)
* Aggregation of values by:
	* Frequency
	* Summation
	* Earliest/Latest occurrence 
	* Highest/Lowest overall occurrence 
* Filter results by threshold values (e.g. top 20 or around current player's rank) or [Relationships](relationship.md)

## API
* Client
    * [LeaderboardClient](xref:PlayGen.SUGAR.Client.LeaderboardClient)
* Contracts
    * [LeaderboardRequest](xref:PlayGen.SUGAR.Contracts.LeaderboardRequest)
    * [LeaderboardResponse](xref:PlayGen.SUGAR.Contracts.LeaderboardResponse)
    * [LeaderboardStandingsRequest](xref:PlayGen.SUGAR.Contracts.LeaderboardStandingsRequest)
    * [LeaderboardStandingsResponse](xref:PlayGen.SUGAR.Contracts.LeaderboardStandingsResponse)
    * [LeaderboardType](xref:PlayGen.SUGAR.Contracts.LeaderboardType)
    * [LeaderboardFilterType](xref:PlayGen.SUGAR.Contracts.LeaderboardFilterType)
    * [GameDataType](xref:PlayGen.SUGAR.Contracts.GameDataType)
    * [CriteriaScope](xref:PlayGen.SUGAR.Contracts.CriteriaScope)
    * [ActorType](xref:PlayGen.SUGAR.Contracts.ActorType)

* WebAPI
    * [LeaderboardController](xref:PlayGen.SUGAR.WebAPI.Controllers.LeaderboardsController)

## Examples
* Create a leaderboard
	This example shows how to create a leaderboard which will display the highest rankings for the combined total of kingdoms the players have conquered. This uses [LeaderboardClient](xref:PlayGen.SUGAR.Client.LeaderboardClient)'s Create function, passing a [LeaderboardRequest](xref:PlayGen.SUGAR.Contracts.LeaderboardRequest) as the parameter. This request will specify the identifier token "MOST_KINGDOMS_CONQUERED", key for which to check in [GameData](gameData.md) "KingdomsConquered" and [LeaderboardType](xref:PlayGen.SUGAR.Contracts.LeaderboardType) Cumulative to add all the values of every entry matching the key. The code will then store the Token of the leaderboard from the [LeaderboardResponse](xref:PlayGen.SUGAR.Contracts.LeaderboardResponse) for later use.

```cs
		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private LeaderboardClient _leaderboardClient;
		private int _gameId;
		private string _leaderboardToken;

		private void CreateLeaderboard() 
		{
			// create instance of the leaderboard client
			_leaderboardClient = sugarClient.Leaderboard;

			// create a LeaderboardRequest
			var leaderboardRequest = new LeaderboardRequest 
			{
				GameId = _gameId,
				Name = "Most Kingdoms Conquered",
				Token = "MOST_KINGDOMS_CONQUERED",
				Key = "KingdomsConquered",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType	= LeaderboardType.Cumulative
			};

			// create the leaderboard and store the response
			var leaderboardResponse = _leaderboardClient.Create(leaderboardRequest);

			// store the token of the leaderboard for use in other functions
			_leaderboardToken = leaderboardResponse.Token;
		}
```

* Get standings for a leaderboard
	To display the leaderboard inside the game, the current standings of the leaderboard must be retreived. The [LeaderboardClient](xref:PlayGen.SUGAR.Client.LeaderboardClient)'s CreateGetLeaderboardStandings function is called by passing a [LeaderboardStandingsRequest](xref:PlayGen.SUGAR.Contracts.LeaderboardStandingsRequest) object as the parameter, which specifies filters for the returned results. This example will retreive the 8 rankings nearest to the player by setting the [LeaderboardFilterType](xref:PlayGen.SUGAR.Contracts.LeaderboardFilterType) to Near, the limit to 8 and offset as 0. The returned [LeaderboardStandingsResponse](xref:PlayGen.SUGAR.Contracts.LeaderboardStandingsResponse) contains all the data to populate the visual representation of the leaderboard. 


```cs
		private void GetLeaderboardStadings() 
		{
			// create a LeaderboardStandingsRequest
			var leaderboardStandingsRequest = new LeaderboardStandingsRequest 
			{
				LeaderboardToken = _leaderboardToken,
				GameId = _gameId,
				ActorId = _userId,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				Limit = 8,
				Offset = 0
			};

			// retreive the standings and store the responses
			var leaderboardStandingsResponse = _leaderboardClient.CreateGetLeaderboardStandings(leaderboardStandingReqeusts);

			// output the leaderboard
			foreach (var standing in leaderboardStandings)
			{
				Console.WriteLine(standing.Ranking.ToString() + " | " + standing.ActorName + " | Conquered: " + standing.Value);
			}
		}
```