using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Common.Shared
{
	/// <summary>
	/// Encapsulates the reward given for completing an achievement or skill.
	/// </summary>
	public class Reward
	{
		/// <summary>
		/// The key which will be stored in SaveData.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string SaveDataKey { get; set; }

		/// <summary>
		/// SaveDataCategory of the value for this SaveData.
		/// </summary>
		[Required]
		public SaveDataCategory SaveDataCategory { get; set; }

		/// <summary>
		/// SaveDataType of the value for this SaveData.
		/// </summary>
		[Required]
		public SaveDataType SaveDataType { get; set; }

		/// <summary>
		/// The value of the SaveData.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Value { get; set; }
	}
}