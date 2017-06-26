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
		Actor,

		/// <summary>
		/// Test the criteria against the specified actors relations
		/// ie. a users friends, or a groups members
		/// </summary>
		RelatedActors,

		//TODO: decide whether there is a valid use case for testing a group relations, eg. alliance
	}
}