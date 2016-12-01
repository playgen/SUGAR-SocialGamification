using System.ComponentModel.DataAnnotations;

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
        [Required]
		[StringLength(64)]
		public string Key { get; set; }

        /// <summary>
        /// SaveDataType of the value for this GameData.
        /// </summary>
        [Required]
        public SaveDataType DataType { get; set; }

        /// <summary>
        /// The value of the GameData.
        /// </summary>
        [Required]
		[StringLength(64)]
		public string Value { get; set; }
	}
}