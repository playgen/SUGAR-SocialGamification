using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates User specific operations.
	/// </summary>
	public class UserClient : ClientBase
	{
		public UserClient(string baseAddress) : base(baseAddress)
		{
		}

		/// <summary>
		/// Get a list of all Users.
		/// </summary>
		/// <returns>A list of <see cref="ActorResponse"/> that hold User details.</returns>
		public IEnumerable<ActorResponse> Get()
		{
			var query = GetUriBuilder("api/user").ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		/// <summary>
		/// Get a list of Users that match <param name="name"/> provided.
		/// </summary>
		/// <param name="name">Array of User names.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> Get(string name)
		{
			var query = GetUriBuilder($"api/user/find/{name}").ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		/// <summary>
		/// Get User that matches <param name="id"/> provided.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns><see cref="ActorResponse"/> which matches search criteria.</returns>
		public IEnumerable<ActorResponse> Get(int id)
		{
			var query = GetUriBuilder($"api/user/findbyid/{id}").ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		/// <summary>
		/// Create a new User.
		/// Requires the <see cref="ActorRequest.Name"/> to be unique for Users.
		/// </summary>
		/// <param name="actor"><see cref="ActorRequest"/> object that holds the details of the new User.</param>
		/// <returns>A <see cref="ActorResponse"/> containing the new User details.</returns>
		public ActorResponse Create(ActorRequest actor)
		{
			var query = GetUriBuilder("api/user").ToString();
			return Post<ActorRequest, ActorResponse>(query, actor);
		}

		/// <summary>
		/// Delete User with the <param name="id"/> provided.
		/// </summary>
		/// <param name="id">User ID.</param>
		public void Delete(int id)
		{
			var query = GetUriBuilder($"api/user/{id}").ToString();
			Delete(query);
		}
	}
}
