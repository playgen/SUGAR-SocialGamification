# Skills
Skills represent a players proficiency or ability in a . SUGAR allows the game designer to define and track which skills the game is designed to teach.

Such a skill is globally defined with a game-specific criteria. The criteria checks the [GameData](/articles/GameData) table for occurences that serve as evidence of that skill's demonstration.

## Features
* Get all skills
* Get all skills that match a name/id
* Get all skills associated with a particular game
* Get a player's performance of a particular skill
* Can be global or game-specific
* CRUD Skill
* CRUD Skill Metadata
* Search Skill (ID/name/metadata/Actor)


## API
* Client
    * [SkillClient](xref:PlayGen.SUGAR.Client.SkillClient)
* Contracts

* WebAPI
    * [SkillController](xref:PlayGen.SUGAR.WebAPI.Controllers.SkillsController)
