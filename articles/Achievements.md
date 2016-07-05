# Achievements
An achievement is an attainable meta-goal. Achievements can be used to facilitate levels of recognition. 
SUGAR allows developers to add achievements for groups or users to a game. These achievements are defined with a criteria which must be met in order to complete them. This is done by checking against entires in the [GameData](/articles/GameData) table. Developers have the ability to fetch a list of achievements for each game and delete the achievements from the system. Progress can also be calculated and completion saved by checking against data held within 

## Features
* Get all achievements
* Get all achievements for a specific game
* Get all achievement progression of a particular actor within a game
* Get a particular actor's progress of a specific achievement 
* Create a new achievement 
* Delete an achievement



## API
* ClientAPI
    * [AchievementClient](/api/PlayGen.SUGAR.ClientAPI.AchievementClient)
* Contracts
    * [AchievementCriteria](/api/PlayGen.SUGAR.Contracts.AchievementCriteria)
    * [AchievementProgressResponse](/api/PlayGen.SUGAR.Contracts.AchievementProgressResponse)
    * [AchievementRequest](/api/PlayGen.SUGAR.Contracts.AchievementRequest)
    * [AchievementResponse](/api/PlayGen.SUGAR.Contracts.AchievementResponse)
* WebAPI
    * [AchievementController](/api/PlayGen.SUGAR.WebAPI.Controllers.AchievementController)
