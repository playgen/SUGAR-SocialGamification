namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates group details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// "Name" : "Group Name",
	/// "Description" : "Group Description"
	/// }
	/// </example>
	public class GroupRequest : ActorRequest
    {
	    /// <summary>
	    /// Optional game id that this group belongs to.
	    /// </summary>
	    public int? GameId { get; set; }
	}
}