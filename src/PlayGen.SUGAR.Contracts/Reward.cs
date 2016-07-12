using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

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
	public class Reward
	{
		/// <summary>
		/// The key which will be stored in GameData.
		/// </summary>
		[Required]
		public string Key { get; set; }

		/// <summary>
		/// GameDataType of the value for this GameData.
		/// </summary>
		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType DataType { get; set; }

		/// <summary>
		/// The value of the GameData.
		/// </summary>
		[Required]
		public string Value { get; set; }
	}
}