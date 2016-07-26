---
uid: skill
---

# Skills
Skills represent a players proficiency or ability. SUGAR allows the game designer to define and track which skills the game is designed to teach.

Such a skill is globally defined with a game-specific criteria. The criteria checks the [GameData](gameData.md) table for occurrences that serve as evidence of that skill's demonstration.

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
 	* [AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria)
    * [AchievementProgressResponse](xref:PlayGen.SUGAR.Contracts.AchievementProgressResponse)
    * [AchievementRequest](xref:PlayGen.SUGAR.Contracts.AchievementRequest)
    * [AchievementResponse](xref:PlayGen.SUGAR.Contracts.AchievementResponse)
    * [GameDataType](xref:PlayGen.SUGAR.Contracts.GameDataType)
    * [CriteriaQueryType](xref:PlayGen.SUGAR.Contracts.CriteriaQueryType)
    * [ComparisonType](xref:PlayGen.SUGAR.Contracts.ComparisonType)
    * [ActorType](xref:PlayGen.SUGAR.Contracts.ActorType)
    * [CriteriaScope](xref:PlayGen.SUGAR.Contracts.CriteriaScope)
* WebAPI
    * [SkillController](xref:PlayGen.SUGAR.WebAPI.Controllers.SkillsController)

## Examples
* Create a skill
	Skills work identically to [Achievements](achievement.md), utilising the same contracts and [GameData](gameData.md). This example shows how to set up the Swordsmanship skill for a game. The skill has an [AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria) specifying the value and ComparisonType to determine at which point the skill has been learnt, key "Swordsmanship" and CriteriaQueryType to sum all GameData entry values matching the key.

```cs
		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private SkillClient _skillClient;
		private int _gameId;

		private void SetUpSkill()
		{
			// create instance of the achievement client
			_skillClient = sugarClient.Skill;
			
			// create an AchievementCriteria list
			var achievementCriteria = new List<AchievementCriteria>()
			{
				new AchievementCriteria()
				{
					DataType = GameDataType.Long,
					Value = "100",
					Key = "Swordsmanship",
					CriteriaQueryType = CriteriaQueryType.Sum,
					ComparisonType = ComparisonType.GreaterOrEqual,
					Scope = CriteriaScope.Actor
				}
			};
			
			// place the criteria inside an AchievementRequest
			var achievementRequest = new AchievementRequest()
			{
				GameId = _gameId,
				Name = "Swordsmanship Skill!",
				ActorType = ActorType.User,
				Token = "swordsmanship",
				CompletionCriteria = achievementCriteria
			};

			// create the skill
			_skillClient.Create(achievementRequest);
		}

```

* Submitting data for when somthing which may be used to evaluate progress towards a Skill
	
	A skill uses keys in [GameData](gameData.md) that match its [AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria). This data is submitted at points in the game which demonstrate progress towards the skill (as well as other uses).

```cs
		private void SwingSword()
		{
			// *Sword Swinging Code* //


			// create instance of GameDataClient
			var gameDataClient = sugarClient.GameData;

			// create GameDataRequest
			var gameDataRequest = new GameDataRequest()
			{
				GameId = _gameId,
				ActorId = _userId,
				GameDataType = GameDataType.Long,
				Value = "1",
				Key = "Swordsmanship"
			};

			// add the GameData
			gameDataClient.Add(gameDataRequest);
		}
```

* Checking a skill's progress

	Using the [SkillClient](xref:PlayGen.SUGAR.Client.SkillClient)'s GetAchievementProgress function and specifying the GameId, ActorId and Token returns an [AchievementProgressResponse](xref:PlayGen.SUGAR.Contracts.AchievementProgressResponse) object for that Actor's progress towards the skill in that game. 

```cs
		private float CheckSkillProgress()
		{
			// Check the user's progress towards the achievements in the specified game
			var achievementProgressResponse = _skillClient.GetAchievementProgress
			(
				"swordsmanship", 
				_gameId,
				 _userId
			 );

			// Output the progress
			return achivementProgressResponse.Progress;
		}
```