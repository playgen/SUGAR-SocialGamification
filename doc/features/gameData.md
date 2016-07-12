# GameData
GameData is a storage system, following a key-value structure. It is used by SUGAR features and provides flexibility in providing custom game-specific storage solutions.

GameData provides storage for [Achievements](/features/Achievements.html), [Skills](/features/Skills.html), [Resources](/features/Resources.html) and custom general data. 

## Features
* CRD GameData 
* Search GameData (by ID/Game/Actor/Skill)

## GameData Categories
- General Data - All information that does not fit into any other category.
    - Example - A [user](/features/user.html) gaining 10 points in level 2.
- Resources - Long-only data which can be transferred to and from [actors](/features/actor.html) within the system.
    - Example - A user collecting 10 gold, a consumable item within the game. 
- Achievements - Stored completion of all [AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria) for an achievement.
    - Example - A user meeting the criteria for the achievement "Score 10,000 points" in a game.
- Skills - Stored completion of all [AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria) for an skill.
    - Example - A user meeting the criteria for the "Social" skill in a game.

## API
* Client
    * [GameDataClient](xref:PlayGen.SUGAR.Client.GroupMemberClient)
* Contracts
    * [GameDataRequest](xref:PlayGen.SUGAR.Contracts.GameDataRequest)
    * [GameDataResponse](xref:PlayGen.SUGAR.Contracts.GameDataResponse)
* WebAPI
    * [GameDataController](xref:PlayGen.SUGAR.WebAPI.Controllers.GameDataController)