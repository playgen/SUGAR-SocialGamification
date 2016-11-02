namespace PlayGen.SUGAR.Contracts.Shared
{
    /// <summary>
    /// Encapsulates requirements for completing an achievement or skill.
    /// </summary>
    /// <example>
    /// JSON
    /// {
    /// Key : "GameData Key",
    /// DataType : "String",
    /// CriteriaQueryType : "Any",
    /// ComparisonType : "Equals",
    /// Scope : "Actor",
    /// Value : "GameData Key Value"
    /// }
    /// </example>
    public class CompletionCriteria : Common.Shared.CompletionCriteria
    {
        // todo make all fields required for contracts
    }
}
