---
uid: user
---

# Users
Users are individuals interacting with the system. They may fulfil a range of roles including player, game master, game admin, teacher or system admin. Users can have metadata associated with them, such as nice name, profile image and bio.

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
    * <xref:PlayGen.SUGAR.Common.Shared.EvaluationCriteria>
    * [AchievementProgressResponse](xref:PlayGen.SUGAR.Contracts.Shared.EvaluationProgressResponse)
    * [ActorRequest](xref:PlayGen.SUGAR.Contracts.Shared.ActorRequest)
    * [ActorResponse](xref:PlayGen.SUGAR.Contracts.Shared.ActorResponse)
* WebAPI
    * [UserController](xref:PlayGen.SUGAR.WebAPI.Controllers.UserController)

## Roadmap

* Integration of permission system.
Providing the ability to set system or game specific permission across the platform functionalities.  

* User id mapping.
Providing the ability for an individual to have multiple usernames depending on their role, or connect with one or more social media accounts for example for authentication. 