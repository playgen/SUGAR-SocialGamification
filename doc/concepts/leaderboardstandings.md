# LeaderboardStandings

[LeaderboardStandings](xref:PlayGen.SUGAR.Contracts.LeaderboardStandingsRequest) are used to obtain a collection of [users](/features/user.html) or [groups](/features/group.html) based on a specific Leaderboard.

In order to setup a LeaderboardStandings, the following must be passed:

- LeaderboardToken - Used to identify the specific [leaderboard](/features/leaderboard.html) that this LeaderboardStandings applies to.

- GameId - The [game](/features/game.html) that this LeaderboardStandings and leaderboard is specific to.

- actorId - The [actor](/features/actor.html) that we are most concerned with. The actor is used in conjunction with the LeaderboardFilterType.

- [LeaderboardFilterType](xref:PlayGen.SUGAR.Contracts.LeaderboardFilterType) - Allows you to specify how you want the results returned in relation to the actor:
	- Top: Returns the top globally ranked actors.
	- Near: Returns the actors in relation to the actorId Provided. This works in conjunction with the Offset. By default it will return a collection where the specified actor is in the middle.
	- Friends: If the actor is a user, only the friends of that user are returned.
	- groupMembers: If the actor is a group, only the members of that group are returned.

- Limit: The maximum amount of rankings to return.

- Offset: Used in conjunction with the LeaderboardFilterType Near.
	- Example 1 - if the actorId = a user's Id, LeaderboardFilterType = Near, Limit = 100 and Offset = 0, a collection of rankins will be returned where the user is at position 50, with 49 rakings before and 10 after.
	- Example 2 - if the actorId = a user's Id, LeaderboardFilterType = Near, Limit = 100 and Offset = 10, a collection of rankins will be returned where the user is at position 40, with 39 rankings before and 60 after.

- DateStart: Because the leaderboard associated with the LeaderboardStandings queries the [GameData](/features/gamedata.html) of actors to determine their ranking; you can specify a StartDate and EndDate where on the GameData added to the system during that time span will be considered in determining actors' rankings.

- DateEnd: The end of the date range for gameData to be considered in determining an actor's ranking. Both the DateStart and DateEnd can be left empty in which case all of the gameData for the actors will be considered.

## Examples 
```
LeaderboardToken = "Player High Score",  
GameId = 5,  
actorId = 23,  
LeaderboardFilterType = "Near",  
Limit = 200,  
Offset = 50,  
DateStart = null,  
DateEnd = null,  
```