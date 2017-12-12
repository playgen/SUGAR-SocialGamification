namespace PlayGen.SUGAR.Common
{
	/// <summary>
	/// Enum for selecting if GameData is to checked against just the actor in question or other actors relating to them (i.e. group members)
	/// </summary>
	public enum CriteriaScope
	{
		/// <summary>
		/// Test the criteria against the specified actor
		/// </summary>
		Actor = 0,
		/// <summary>
		/// Test the criteria against the specified actors user relations
		/// ie. a users friends, or a groups members
		/// </summary>
		RelatedUsers = 1,
		/// <summary>
		/// Test the criteria against the specified actors group relations
		/// ie. a groups alliances
		/// </summary>
		RelatedGroups = 2,
		/// <summary>
		/// Test the criteria against the specified actors groups user relations
		/// ie. the group members within a groups alliances
		/// </summary>
		RelatedGroupUsers = 3
	}
}