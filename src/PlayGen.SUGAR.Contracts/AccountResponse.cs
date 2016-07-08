using System;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates user and token details at log-in.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// User : {
	/// Id : 1,
	/// Name : "Name"
	/// },
	/// Token : "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyaWQiOjQsIm5vdGJlZm9jE0Njc5MTM3MzguMH0.We4TPoujGdEFtqLZWCMPqFr9EjYWlmKBJMQ61YAczkYyZSI6MTQ4MzgwNDEzOC4wLCJpc3N1ZWRhdCI6MTQ2NzkwNjUzOC4wLCJleHBpcnkiO"
	/// }
	/// </example>
	public class AccountResponse
	{
		public ActorResponse User { get; set; }
	}
}
