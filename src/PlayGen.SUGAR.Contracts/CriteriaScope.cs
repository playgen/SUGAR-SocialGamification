namespace PlayGen.SUGAR.Contracts
{
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