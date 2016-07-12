# Users
Users are individuals interacting with the system. They may fulfil a range of roles including player, game master, game admin, teacher or system admin. 
Users can have metadata associated with them, such as nice name, profile image and bio.

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
    * [AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria)
    * [AchievementProgressResponse](xref:PlayGen.SUGAR.Contracts.AchievementProgressResponse)
    * [ActorRequest](xref:PlayGen.SUGAR.Contracts.ActorRequest)
    * [ActorResponse](xref:PlayGen.SUGAR.Contracts.ActorResponse)
* WebAPI
    * [UserController](xref:PlayGen.SUGAR.WebAPI.Controllers.UserController)

## Roadmap

* Integration of permission system.
Providing the ability to set system or game specific permission across the platform functionalities.  

* User id mapping.
Providing the ability for an individual to have multiple usernames depending on their role, or connect with one or more social media accounts for example for authentication. 