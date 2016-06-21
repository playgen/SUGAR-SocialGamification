namespace PlayGen.SGA.Contracts
{
    /// <summary>
    /// Encapsulates requirements for completing an achievement.
    /// </summary>
    public class AchievementCriteria
    {
        public string Key { get; set; }

        public DataType DataType { get; set; }

        public ComparisonType ComparisonType { get; set; }

        public string Value { get; set; }
    }
}
