---
uid: skill
---

# Skills
Skills represent a players proficiency or ability. SUGAR allows the game designer to define and track which skills the game is designed to teach.

Such a skill is globally defined with a game-specific criteria. The criteria checks the [GameData](gameData.md) table for occurrences that serve as evidence of that skill's demonstration.

# Note
Both Skills and [Achievements](achievement.md) build on "Evaluations" which may contain a multitude of "EvaluationCriteria", specifiying the conditions that need to be satisfied for this specific Evaluation to be considered complete.

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
    * <xref:PlayGen.SUGAR.Client.SkillClient>
* Contracts
 	* <xref:PlayGen.SUGAR.Common.Shared.EvaluationCriteria>
    * <xref:PlayGen.SUGAR.Contracts.Shared.EvaluationProgressResponse>
    * <xref:PlayGen.SUGAR.Contracts.Shared.EvaluationCreateRequest>
    * <xref:PlayGen.SUGAR.Contracts.Shared.EvaluationUpdateRequest>
    * <xref:PlayGen.SUGAR.Contracts.Shared.EvaluationResponse>
    * <xref:PlayGen.SUGAR.Common.Shared.SaveDataType>
    * <xref:PlayGen.SUGAR.Common.Shared.CriteriaQueryType>
    * <xref:PlayGen.SUGAR.Common.Shared.ComparisonType>
    * <xref:PlayGen.SUGAR.Common.Shared.ActorType>
    * <xref:PlayGen.SUGAR.Common.Shared.CriteriaScope>
* WebAPI
    * <xref:PlayGen.SUGAR.WebAPI.Controllers.SkillsController>

## Examples
* Create a skill
	Skills work identically to [Achievements](achievement.md), utilising the same contracts and [GameData](gameData.md). This example shows how to set up the Swordsmanship skill for a game. The skill has an [<xref:PlayGen.SUGAR.Common.Shared.EvaluationCriteria> specifying the value and ComparisonType to determine at which point the skill has been learnt, key "Swordsmanship" and CriteriaQueryType to sum all GameData entry values matching the key.

```cs
		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private SkillClient _skillClient;
		private int _gameId;

		private void SetUpSkill()
		{
			// create instance of the achievement client
			_skillClient = sugarClient.Skill;
			
			// create an EvaluationCriteria list
			var EvaluationCriteria = new List<EvaluationCriteria>()
			{
				new EvaluationCriteria()
				{
					DataType = GameDataType.Long,
					Value = "100",
					Key = "Swordsmanship",
					CriteriaQueryType = CriteriaQueryType.Sum,
					ComparisonType = ComparisonType.GreaterOrEqual,
					Scope = CriteriaScope.Actor
				}
			};
			
			// place the criteria inside an EvaluationCreateRequest
			var EvaluationCreateRequest = new EvaluationCreateRequest()
			{
				GameId = _gameId,
				Name = "Swordsmanship Skill!",
				ActorType = ActorType.User,
				Token = "swordsmanship",
				CompletionCriteria = EvaluationCriteria
			};

			// create the skill
			_skillClient.Create(EvaluationCreateRequest);
		}

```

* Submitting data for when somthing which may be used to evaluate progress towards a Skill
	
	A skill uses keys in [GameData](gameData.md) that match its <xref:PlayGen.SUGAR.Common.Shared.EvaluationCriteria>. This data is submitted at points in the game which demonstrate progress towards the skill (as well as other uses).

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

* Automatically recieve skill progress completion notifications:

 Enable and use automatic notifications:

```cs
		_skillClient.EnableNotifications(true);
```
	
  And then poll to see if any achievements have been recieved.

```cs
		EvaluationNotification notification;
		if(_skillClient.TryGetPendingNotification(out notification))
		{
			// There was a penging skill notification, so do something with it
			Log.Info($"Got skill notification: {notification.Name} " + 
				$"with progress: {notification.Progress}");
		}
```

* Requesting specific using the [SkillClient](xref:PlayGen.SUGAR.Client.SkillClient)'s GetSkillProgress function and specifying the GameId, ActorId and Token returns an <xref:PlayGen.SUGAR.Contracts.Shared.EvaluationProgressResponse> object for that Actor's progress towards the skill in that game. 

```cs
		private float CheckSkillProgress()
		{
			// Check the user's progress towards the achievements in the specified game
			var skillProgressResponse = _skillClient.GetSkillProgress
			(
				"swordsmanship", 
				_gameId,
				 _userId
			 );

			// Output the progress
			return skillProgressResponse.Progress;
		}
```