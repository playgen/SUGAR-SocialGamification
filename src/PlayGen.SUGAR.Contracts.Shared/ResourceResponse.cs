namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	///     Encapsulates resource details from the server.
	/// </summary>
	public class ResourceResponse : IResponse
	{
		/// <summary>
		///     The id of the Resource.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///     The id of the Actor which this Resource relates to.
		/// </summary>
		public int? ActorId { get; set; }

		/// <summary>
		///     The id of the Game which this Resource relates to.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		///     The identifier/name of the Resource.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		///     The amount of the Resource belonging to the actor/game.
		/// </summary>
		public long Quantity { get; set; }
	}
}