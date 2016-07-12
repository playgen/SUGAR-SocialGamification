# AchievementCriteria

[AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria) are the goals which [users](/features/user.html) and [groups](/features/group.html) are set in order to complete [achievements](/features/achievement.html) and [skills](/features/skill.html). At least one must be assigned and only information stored within [GameData](/features/gameData.html) can be queried against.

In order to set up an [AchievementCriteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria), the following must be passed:

- Key - The fundermental piece of any criteria. All data collected will require the key to match with what is given here.

- DataType - The type of data being queried against. As with key, only data that matches this type will be collected.
    - Example - if "String" is given as [GameDataType](xref:PlayGen.SUGAR.Contracts.GameDataType), only data also stored as "String" will be checked against.

- Scope - The range of data collected for this [actor](/features/actor.html).
    - The "Actor" scope only looks at data stored against the actor directly.
    - "RelatedActors" will collect data that have a relationship with the provided actor.
    - Example - "RelatedActor" could be used with a group to collect data for all members of that group.

- Value - The target value which will be checked evaluated against. Must always be passed as a string.

- [ComparisonType](xref:PlayGen.SUGAR.Contracts.ComparisonType) - How the retrieved data will be compared against the target value. If the retrieved data compared to the value matches the ComparisonType provided, then the criteria has been met.
    - Example -  if the retrieved data equals 20, the value is 15 and the ComparisonType is "GreaterThan", the criteria has been met. If the ComparisonType was "Equals" then the criteria would not have been met.

- [CriteriaQueryType](xref:PlayGen.SUGAR.Contracts.CriteriaQueryType) - The type of query that will be performed against the collected data.
    - "Any" checks to see if the criteria has been ever met by the collected data.
    - "Latest" checks if the last data that matched Key and GameDataType met the criteria.
    - "Sum" is used by numeric GameDataTypes (Long and Float) and adds the value of all collected data together.

## Examples

   Key = "Gold",  
   DataType = "Long",  
   Scope = "Actor",  
   Value = "15",  
   ComparisonType = "GreaterOrEqual",  
   CriteriaQueryType = "Sum"  

   Key = "Level_1_Time",  
   DataType = "Float",  
   Scope = "Actor",  
   Value = "9.25",  
   ComparisonType = "Less",  
   CriteriaQueryType = "Any"  



