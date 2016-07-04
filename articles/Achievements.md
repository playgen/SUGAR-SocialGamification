# Achievements
SUGAR allows developers to add achievements for either groups or users to a game, with these achievements able to contain multiple different criteria in order to complete them. Developers can also fetch a list of achievements for each game and delete the achievements from the system. As part of the achievement section of the system, progress can be calculated and completion saved by checking against data held within [SaveData](/articles/SaveData).

## Features
* Get all achievements
* Get all achievements for a specific game
* Creation
* Deletion
* Get all achievement progression of a particular actor within a game
* Get a particular actor's progress of a specific achievement 


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
