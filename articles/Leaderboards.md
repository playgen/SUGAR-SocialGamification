# Leaderboards
Generating and filtering leaderboards based on [GameData](/articles/GameData), is handled within SUGAR's API. 

## Features
* Generate by game, key or actorType.
* Aggregation of values by frequency, summation, earliest/latest occurance or highest/lowest overall occurance. 
* Filter results by threshold values (e.g. top 20 or around current player's rank), [Relationships](/articles/Relationships),   

## API
* ClientAPI
    * [LeaderboardClient](/api/PlayGen.SUGAR.ClientAPI.LeaderboardClient)
* Contracts
    * [LeaderboardRequest](/api/PlayGen.SUGAR.Contracts.LeaderboardRequest)
    * [LeaderboardResponse](/api/PlayGen.SUGAR.Contracts.LeaderboardResponse)
    * [LeaderboardStandingsRequest](/api/PlayGen.SUGAR.Contracts.LeaderboardStandingsRequest)
    * [LeaderboardStandingsResponse](/api/PlayGen.SUGAR.Contracts.LeaderboardStandingsResponse)
* WebAPI
    * [LeaderboardController](/api/PlayGen.SUGAR.WebAPI.Controllers.LeaderboardController)