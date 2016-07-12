# Achievements
Achievements provide a flexible and simple way to increase user engagement, tracking their actions within and across multiple games. Achievements may be viewed as goals that individual or multiple actors can acquire through meeting one or more criteria. 

The criteria is flexible and can be written by the game developer or provided by the platform based on any game specific action. Achievements can be binary or incremental, ie. (complete/not complete) or have levels or percentages of progression toward their completion. They can be global, across all games, or associated with a single game. 

## Features
* CRUD achievements
* Search achievements (ID/Game/Name/Actor)

## API
* Client
    * [AchievementClient](xref:PlayGen.SUGAR.Client.AchievementClient)
* Contracts
    * [AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria)
    * [AchievementProgressResponse](xref:PlayGen.SUGAR.Contracts.AchievementProgressResponse)
    * [AchievementRequest](xref:PlayGen.SUGAR.Contracts.AchievementRequest)
    * [AchievementResponse](xref:PlayGen.SUGAR.Contracts.AchievementResponse)
* WebAPI
    * [AchievementController](xref:PlayGen.SUGAR.WebAPI.Controllers.AchievementsController)

	
## Roadmap

* Portable achievement system.
Many game networks such as the Google play services, Apple gamkits, Microsoft game centre, Facebook game services and Steam Works offer achievements. Integrating multiple achievement systems into a game is time consuming. Additionally platform providers regularly update their APIs, creating a head-ache for keeping up-to-date with multiple systems. The achievement system will provide the ability for the actors’ achievement to be recorded automatically against other 3rd party systems.   

* Challenge system.
Achievement system will be extended by adding temporal (time based) and ownership component. This extension will be referred to as Challenge.  Challenges may be seen as achievements that can be suggested to actors, gifted by one actor to another, accepted or  rejected by an actor, as well as tracked to see if actors attempted or abandoned them. 

