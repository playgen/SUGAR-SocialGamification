namespace PlayGen.SUGAR.Common.Shared
{
    /// <summary>
    /// Encapsulates the reward given for completing an achievement or skill.
    /// </summary>
	public class Reward
	{
        /// <summary>
        /// The key which will be stored in GameData.
        /// </summary>
        //[Required]
        public string Key { get; set; }

        /// <summary>
        /// GameDataType of the value for this GameData.
        /// </summary>
        //[Required]
        public GameDataType DataType { get; set; }

        /// <summary>
        /// The value of the GameData.
        /// </summary>
        //[Required]
        public string Value { get; set; }
	}
}