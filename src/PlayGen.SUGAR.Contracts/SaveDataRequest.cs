namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates savedata details.
	/// </summary>
	public class SaveDataRequest
	{
		public int ActorId { get; set; }

		public int GameId { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public GameDataValueType GameDataValueType { get; set; }
	}
}
