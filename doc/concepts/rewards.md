---
uid: reward
---

# Reward

Rewards can be provided to [Users](xref:user) and [Groups](xref:group) upon completion of [Achievements](xref:achievement) and [Skills](xref:skill). Rewards given are stored within GameData, meaning they can in theory be used toward further <xref:PlayGen.SUGAR.Common.Shared.EvaluationCriteria> and [Leaderboards](xref:leaderboard).

In order to set up aa <xref:PlayGen.SUGAR.Common.Shared.Reward>, the following must be passed:

- Key - The unique identifier for the reward being provided within GameData.

- DataType - The <xref:PlayGen.SUGAR.Common.Shared.EvaluationDataType> of data being stored.

- Value - The value which is being stored.

## Examples

   Key = "Gold",  
   DataType = "Long",  
   Value = "15"  

   Key = "Bonus_Points",  
   DataType = "Float",  
   Value = "2.5",  
