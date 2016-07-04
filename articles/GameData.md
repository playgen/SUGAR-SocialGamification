# GameData
SUGAR stores all data from [Achievements](/articles/Achievements), [Leaderboards](/articles/Leaderboards), [Skills](/articles/Skills) and [Resources](/articles/Resources) as GameData. The GameData system also provides flexibility to developers by allowing the storage of custom, game-specific data in the SUGAR platform. GameData uses a key-value structure.

## Features
* Retrieve all GameData entries relating to a specific game and actor, matching the provided key.
* Creation

## API
* ClientAPI
    * [GameDataClient](/api/PlayGen.SUGAR.ClientAPI.GroupMemberClient)
* Contracts
    * [GameDataRequest](/api/PlayGen.SUGAR.Contracts.GameDataRequest)
    * [GameDataResponse](/api/PlayGen.SUGAR.Contracts.GameDataResponse)
* WebAPI
    * [GameDataController](/api/PlayGen.SUGAR.WebAPI.Controllers.GameDataController)