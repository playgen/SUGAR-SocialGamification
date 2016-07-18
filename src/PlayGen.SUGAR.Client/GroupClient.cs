using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Group specific operations.
	/// </summary>
	public class GroupClient : ClientBase
	{
		public GroupClient(string baseAddress, Credentials credentials) : base(baseAddress, credentials)
		{
		}

		/// <summary>
		/// Get a list of all Groups.
		/// </summary>
		/// <returns>A list of <see cref="ActorResponse"/> that hold Group details.</returns>
		public IEnumerable<ActorResponse> Get()
		{
			var query = GetUriBuilder("api/group/list").ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		/// <summary>
		/// Get a list of Groups that match <param name="name"/> provided.
		/// </summary>
		/// <param name="name">Group name.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> Get(string name)
		{
			var query = GetUriBuilder($"api/group/find/{name}").ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		/// <summary>
		/// Get Group that matches <param name="id"/> provided.
		/// </summary>
		/// <param name="id">Group id.</param>
		/// <returns><see cref="ActorResponse"/> which matches search criteria.</returns>
		public ActorResponse Get(int id)
		{
			var query = GetUriBuilder($"api/group/findbyid/{id}").ToString();
			return Get<ActorResponse>(query, new System.Net.HttpStatusCode[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Create a new Group.
		/// Requires the <see cref="ActorRequest.Name"/> to be unique for Groups.
		/// </summary>
		/// <param name="actor"><see cref="ActorRequest"/> object that holds the details of the new Group.</param>
		/// <returns>A <see cref="ActorResponse"/> containing the new Group details.</returns>
		public ActorResponse Create(ActorRequest actor)
		{
			var query = GetUriBuilder("api/group").ToString();
			return Post<ActorRequest, ActorResponse>(query, actor);
		}

		/// <summary>
		/// Update an existing Group.
		/// </summary>
		/// <param name="id">Id of the existing Group.</param>
		/// <param name="group"><see cref="ActorRequest"/> object that holds the details of the Group.</param>
		public void Update(int id, ActorRequest group)
		{
			var query = GetUriBuilder($"api/group/update/{id}").ToString();
			Put(query, group);
		}

		/// <summary>
		/// Delete group with the <param name="id"/> provided.
		/// </summary>
		/// <param name="id">Group ID.</param>
		public void Delete(int id)
		{
			var query = GetUriBuilder($"api/group/{id}").ToString();
			Delete(query);
		}
	}
}
