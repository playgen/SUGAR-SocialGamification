# Leaderboards
A leaderboard is a comparison tool used to visualise the difference in a particular measurement of a group of players. Generating and filtering leaderboards based on [GameData](/articles/GameData), is handled within SUGAR's API. 

## Features
* Generate a leaderboard by:
	* Game
	* ActorType (Group, Player)
	* Key (from GameData)
* Aggregation of values by:
	* Frequency
	* Summation
	* Earliest/Latest occurance 
	* Highest/Lowest overall occurance 
* Filter results by threshold values (e.g. top 20 or around current player's rank) or [Relationships](/articles/Relationships)

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