namespace PlayGen.SGA.Contracts
{
    public class AchievementCriteria
    {
        public string Key { get; set; }

        public DataType DataType { get; set; }

        public ComparisonType ComparisonType { get; set; }

        public string Value { get; set; }
    }
}
