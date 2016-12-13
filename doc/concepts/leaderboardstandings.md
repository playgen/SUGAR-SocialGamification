---
uid: leaderboardStandings
---

# LeaderboardStandings

LeaderboardStandings are used to obtain a collection of [Users](xref:user) or [Groups](xref:group) based on a specific <xref:leaderboard>.

In order to setup a <xref:PlayGen.SUGAR.Contracts.Shared.LeaderboardStandingsRequest>, the following must be passed:

- LeaderboardToken - Used to identify the specific <xref:leaderboard> that this LeaderboardStandings applies to.

- GameId - The <xref:game> that this LeaderboardStandings and leaderboard is specific to.

- ActorId - The <xref:actor> that we are most concerned with. The actor is used in conjunction with the LeaderboardFilterType.

- <xref:PlayGen.SUGAR.Common.Shared.LeaderboardFilterType> - Allows you to specify how you want the results returned in relation to the actor:
	- Top: Returns the top globally ranked actors.
	- Near: Returns the actors in relation to the actorId Provided. This works in conjunction with the PageOffset. By default it will return a collection that contains the provided actor.
	- Friends: If the actor is a user, only the friends of that user are returned.
	- GroupMembers: If the actor is a group, only the members of that group are returned.

- PageLimit: The maximum amount of rankings to return.

- PageOffset: Used in conjunction with the LeaderboardFilterType Near.
	- Example 1 - if the actorId = a user's Id, LeaderboardFilterType = Near, PageLimit = 50 and PageOffset = 0, with the user ranked 80th, a collection of rankings will be returned where the user is at position 30, with 29 rankings before and 20 after.
	- Example 2 - if the actorId = a user's Id, LeaderboardFilterType = Near, PageLimit = 50 and PageOffset = 1, with the user ranked 40th, a collection of rankings between 51th and 100th will be returned, with the user not included.

- DateStart: Because the leaderboard associated with the LeaderboardStandings queries the <xref:gameData> of actors to determine their ranking; you can specify a StartDate and EndDate where on the GameData added to the system during that time span will be considered in determining actors' rankings.

- DateEnd: The end of the date range for gameData to be considered in determining an actor's ranking. Both the DateStart and DateEnd can be left empty in which case all of the gameData for the actors will be considered.

## Examples 
```
LeaderboardToken = "Player High Score",  
GameId = 5,  
actorId = 23,  
LeaderboardFilterType = "Near",  
PageLimit = 20,  
PageOffset = 1,  
DateStart = null,  
DateEnd = null,  
```