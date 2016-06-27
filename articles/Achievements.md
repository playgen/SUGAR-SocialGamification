# Achievements
SGA allows developers to add achievements for either groups or users to a game, with these achievements able to contain multiple different criteria in order to complete them. Developers can also fetch a list of achievements for each game and delete the achievements from the system. As part of the achievement section of the system, progress can be calculated and completion saved by checking against data held within [SaveData](/articles/SaveData).

## Features
* Creation
* Deletion
* Get all achievements for a game

## API
* ClientAPI
    * [Group](/api/PlayGen.SGA.ClientAPI.GroupAchievementClientProxy)
    * [User](/api/PlayGen.SGA.ClientAPI.UserAchievementClientProxy)
* Contracts
    * [AchievementCriteria](/api/PlayGen.SGA.Contracts.AchievementCriteria)
    * [AchievementProgressResponse](/api/PlayGen.SGA.Contracts.AchievementProgressResponse)
    * [AchievementRequest](/api/PlayGen.SGA.Contracts.AchievementRequest)
    * [AchievementResponse](/api/PlayGen.SGA.Contracts.AchievementResponse)
* WebAPI
    * [Group](/api/PlayGen.SGA.WebAPI.Controllers.GroupAchievementController)
    * [User](/api/PlayGen.SGA.WebAPI.Controllers.UserAchievementController)