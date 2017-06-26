namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	///     Encapsulates group details from the server.
	/// </summary>
	/// <example>
	///     JSON
	///     {
	///     Id : 1,
	///     Name : "Actor Name"
	///     }
	/// </example>
	public class GroupResponse : ActorResponse, IResponse
	{
		/// <summary>
		///     The number of members in this group.
		/// </summary>
		public int MemberCount { get; set; }
	}
}