---
uid: criteria
---

# Evalutation Criteria

<xref:PlayGen.SUGAR.Contracts.EvaluationCriteriaCreateRequest> are the goals which a <xref:user> or <xref:group> are set in order to complete an <xref:achievement> or <xref:skill>. At least one must be assigned and only information stored within <xref:evaluationData> can be queried against.

In order to set up an <xref:PlayGen.SUGAR.Contracts.EvaluationCriteriaCreateRequest>, the following must be passed:

- Key - The fundermental piece of any criteria. All data collected will require the key to match with what is given here.

- <xref:PlayGen.SUGAR.Common.EvaluationDataCategory> - The category of EvaluationData being queried against. As with key, only data that matches this category will be collected.
    - Example - if "GameData" is given as EvaluationDataCategory, only data also stored as "GameData" will be checked against.

- <xref:PlayGen.SUGAR.Common.EvaluationDataType> - The type of data being queried against. As with key, only data that matches this type will be collected.
    - Example - if "String" is given as DataType, only data also stored as "String" will be checked against.

- <xref:PlayGen.SUGAR.Common.CriteriaQueryType> - The type of query that will be performed against the collected data.
    - "Any" checks to see if the criteria has been ever met by the collected data.
    - "Latest" checks if the last data that matched Key and GameDataType met the criteria.
    - "Sum" is used by numeric EvaluationDataTypes (Long and Float) and adds the value of all collected data together.
    - "Count" checks if the amount of entries in the database of the specified EvaluationData matches the criteria.

- <xref:PlayGen.SUGAR.Common.ComparisonType> - How the retrieved data will be compared against the target value. If the retrieved data compared to the value matches the ComparisonType provided, then the criteria has been met.
    - Example -  if the retrieved data equals 20, the value is 15 and the ComparisonType is "GreaterThan", the criteria has been met. If the ComparisonType was "Equals" then the criteria would not have been met.

- <xref:PlayGen.SUGAR.Common.CriteriaScope> - The range of data collected for this <xref:actor>. 
    - The "Actor" scope only looks at data stored against the actor directly.
    - "RelatedUsers" will collect data from users that have a relationship with the provided actor.
    - "RelatedGroups" will collect data from groups that have a relationship with the provided actor.
    - "RelatedGroupUsers" will collect data from users who are members of groups that have a relationship with the provided actor.
    - Example - "RelatedUsers" could be used with a group to collect data for all members of that group.

- Value - The target value which will be checked evaluated against. Must always be passed as a string.



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



