---
uid: user
---

# Users
Users are individuals interacting with the system. They may fulfil a range of roles including player, game master, game admin, teacher or system admin. Users can have metadata associated with them, such as nice name, profile image and bio.

A user is public by default but can be made private by updating users with an [ActorRequest](xref:PlayGen.SUGAR.Contracts.ActorRequest). Private users will not show up in leaderboards or searches for other users. 

## Features
* CRUD users
* Search users (name/id)
* CRUD user metadata 
	* User Name
	* User Bio
	* User profile icon 


## API
* Client
    * [UserClient](xref:PlayGen.SUGAR.Client.UserClient)
* Contracts
    * [AchievementProgressResponse](xref:PlayGen.SUGAR.Contracts.EvaluationProgressResponse)
    * [ActorRequest](xref:PlayGen.SUGAR.Contracts.ActorRequest)
    * [ActorResponse](xref:PlayGen.SUGAR.Contracts.ActorResponse)
* WebAPI
    * [UserController](xref:PlayGen.SUGAR.Server.WebAPI.Controllers.UserController)