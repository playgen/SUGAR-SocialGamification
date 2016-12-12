# Rewards

<xref:PlayGen.SUGAR.Common.Shared.Reward> can be provided to [users](/features/user.html) and [groups](/features/group.html) upon completion of [achievements](/features/achievement.html) and [skills](/features/skill.html). Rewards given are stored within GameData, meaning they can in theory be used toward further <xref:PlayGen.SUGAR.Common.Shared.EvaluationCriteria> and [leaderboards](/features/leaderboard.html).

In order to set up aa <xref:PlayGen.SUGAR.Common.Shared.Reward>, the following must be passed:

- Key - The unique identifier for the reward being provided within GameData.

- DataType - The <xref:PlayGen.SUGAR.Common.Shared.SaveDataType> of data being stored.

- Value - The value which is being stored.

## Examples

   Key = "Gold",  
   DataType = "Long",  
   Value = "15"  

   Key = "Bonus_Points",  
   DataType = "Float",  
   Value = "2.5",  
