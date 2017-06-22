namespace PlayGen.SUGAR.Contracts
{
    /// <summary>
    /// Encapsulates the reward given for completing an achievement or skill.
    /// </summary>
    /// <example>
    /// JSON
    /// {
    /// Key : "Reward Key",
    /// DataType : "Float",
    /// Value : "10.5"
    /// }
    /// </example>
    public class RewardCreateRequest : Common.Shared.Reward
    {
        // todo make all fields required for contract
    }
}