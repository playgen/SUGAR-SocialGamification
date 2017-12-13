---
uid:  match
---

# Matches
A match is an entity with a start time, end time, game and creator. 
It can have various data attrubuted to it via the MatchData mechanism.

## Features
* CRUD Match
* Search Matches with filters: game, creator, start time, end time
* Add and Get Match specific data

## API
* Client
    * <xref:PlayGen.SUGAR.Client.MatchClient>
* Contracts
    * <xref:PlayGen.SUGAR.Contracts.MatchResponse>
* WebAPI
    * <xref:PlayGen.SUGAR.Server.WebAPI.Controllers.MatchController>


## Examples
* Create a match
	
```cs
		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private MatchClient _matchClient;
		private int _gameId;

		private void CreateMatch() 
		{
			// create instance of the match client
			_matchClient = sugarClient.Match;
						
			// create the match and store the response
			var matchResponse = _matchClient.Create();

			// store the id of the match for use in other features
			_matchId = matchResponse.Id;
		}
```

* Starting a match

```cs 
		private MatchResponse StartMatch(int matchId) 
		{
			// check for the match and store the response
			var matchResponse = _matchClient.Get(matchId);

			// Start the match
			matchResponse = _matchClient?.Start(matchResponse.Id);
			
			return matchResponse:
		}
```

* Ending a match

```cs 
		private MatchResponse EndMatch(int matchId) 
		{
			// check for the match and store the response
			var matchResponse = _matchClient.Get(matchId);

			// End the match
			matchResponse = _matchClient?.End(matchResponse.Id);

			return matchResponse:
		}
```

* Adding match data

```cs 
		private EvaluationDataResponse AddMatchData(int matchId, int userId) 
		{
			var matchResponse = _matchClient.Get(matchId);

			var matchData = SUGARClient.Match.AddData(new EvaluationDataRequest
            {
                RelatedEntityId = matchResponse.Id,
                GameId = matchResponse.Game.Id,
                CreatingActorId = userId,
                EvaluationDataType = EvaluationDataType.Long,
                Key = "MyMatchScore",
                Value = 100
            });						

			return matchData:
		}
```

* Getting match data

```cs 
		private EvaluationDataResponse GetAllMatchData(int matchId) 
		{
			var matchResponse = _matchClient.Get(matchId);

			var allDataForMatch = SUGARClient.Match.GetData(matchId);

			return allDataForMatch:
		}
```