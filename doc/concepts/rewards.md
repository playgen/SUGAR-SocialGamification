# Rewards

[Rewards](xref:PlayGen.SUGAR.Contracts.Reward) can be provided to [users](/features/user.html) and [groups](/features/group.html) upon completion of [achievements](/features/achievement.html) and [skills](/features/skill.html). Rewards given are stored within GameData, meaning they can in theory be used toward further [achievement criteria](xref:PlayGen.SUGAR.Contracts.AchievementCriteria) and [leaderboards](/features/leaderboard.html).

In order to set up aa [Reward](xref:PlayGen.SUGAR.Contracts.Reward), the following must be passed:

- Key - The unique identifier for the reward being provided within GameData.

- DataType - The [GameDataType](xref:PlayGen.SUGAR.Contracts.GameDataType) of data being stored.

- Value - The value which is being stored.

## Examples

   Key = "Gold",  
   DataType = "Long",  
   Value = "15"  

   Key = "Bonus_Points",  
   DataType = "Float",  
   Value = "2.5",  
