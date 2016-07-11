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