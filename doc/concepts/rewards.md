---
uid: reward
---

# Reward

Rewards can be provided to [Users](xref:user) and [Groups](xref:group) upon completion of [Achievements](xref:achievement) and [Skills](xref:skill). Rewards given are stored within EvaluationData, meaning they can in theory be used toward further [Criteria](xref:criteria) and [Leaderboards](xref:leaderboard).

In order to set up aa <xref:PlayGen.SUGAR.Contracts.RewardCreateRequest>, the following must be passed:

- EvaluationDataKey - The identifier for the reward being provided within EvaluationData.

- EvaluationDataCategory - The <xref:PlayGen.SUGAR.Common.EvaluationDataCategory> of data being stored.

- EvaluationDataType - The <xref:PlayGen.SUGAR.Common.EvaluationDataType> of data being stored.

- Value - The value which is being stored.

## Examples

   EvaluationDataKey = "Gold",
   EvaluationDataCategory = "Resource",  
   EvaluationDataType = "Long",  
   Value = "15"  

   EvaluationDataKey = "Bonus_Points",
EvaluationDataCategory = "GameData",    
   EvaluationDataType = "Float",  
   Value = "2.5",  
