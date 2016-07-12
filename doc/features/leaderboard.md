# Leaderboard
Leaderboards provide a visual comparison tool for measurements of resources or objects associated with actors. A wide variety of leaderboards are supported including those based achievement or resource based on individual, group or single and multiple games. Leaderboards can utilise any [GameData](/articles/GameData), [Actor](/articles/actor),[Skill](/articles/Skills) or [Resource](/articles/Resources). 

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
* WebAPI
    * [LeaderboardController](xref:PlayGen.SUGAR.WebAPI.Controllers.LeaderboardsController)