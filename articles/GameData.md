# GameData
GameData is a storage system, following a key-value structure. It is used by SUGAR features and provides flexibility in providing custom game-specific storage solutions.

SUGAR stores all data from [Achievements](/articles/Achievements), [Leaderboards](/articles/Leaderboards), [Skills](/articles/Skills) and [Resources](/articles/Resources) as GameData. 

## Features
* Retrieve all GameData entries relating to a specific game and actor, matching the provided key
* Create a new GameData entry

## API
* ClientAPI
    * [GameDataClient](/api/PlayGen.SUGAR.ClientAPI.GroupMemberClient)
* Contracts
    * [GameDataRequest](/api/PlayGen.SUGAR.Contracts.GameDataRequest)
    * [GameDataResponse](/api/PlayGen.SUGAR.Contracts.GameDataResponse)
* WebAPI
    * [GameDataController](/api/PlayGen.SUGAR.WebAPI.Controllers.GameDataController)