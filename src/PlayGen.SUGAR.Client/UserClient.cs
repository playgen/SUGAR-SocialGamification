using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates User specific operations.
	/// </summary>
	public class UserClient : ClientBase
	{
		private const string ControllerPrefix = "api/user";

		public UserClient(string baseAddress, IHttpHandler httpHandler, AsyncRequestController asyncRequestController, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, asyncRequestController, evaluationNotifications)
		{
		}

		/// <summary>
		/// Get a list of all Users.
		/// </summary>
		/// <returns>A list of <see cref="UserResponse"/> that hold User details.</returns>
		public IEnumerable<UserResponse> Get()
		{
			var query = GetUriBuilder(ControllerPrefix + "/list").ToString();
			return Get<IEnumerable<UserResponse>>(query);
		}

		/// <summary>
		/// Get a list of Users that match <param name="name"/> provided.
		/// </summary>
		/// <param name="name">Array of User names.</param>
		/// <param name="exactMatch">Match the name exactly.</param>
		/// <returns>A list of <see cref="UserResponse"/> which match the search criteria.</returns>
		public IEnumerable<UserResponse> Get(string name, bool exactMatch = false)
		{
			var query = GetUriBuilder(ControllerPrefix + "/find/{0}/{1}", name, exactMatch).ToString();
			return Get<IEnumerable<UserResponse>>(query);
		}

		/// <summary>
		/// Get User that matches <param name="id"/> provided.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns><see cref="UserResponse"/> which matches search criteria.</returns>
		public UserResponse Get(int id)
		{
			var query = GetUriBuilder(ControllerPrefix + "/findbyid/{0}", id).ToString();
			return Get<UserResponse>(query, new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Create a new User.
		/// Requires the <see cref="UserRequest"/>'s Name to be unique for Users.
		/// </summary>
		/// <param name="actor"><see cref="UserRequest"/> object that holds the details of the new User.</param>
		/// <returns>A <see cref="UserResponse"/> containing the new User details.</returns>
		public UserResponse Create(UserRequest actor)
		{
			var query = GetUriBuilder(ControllerPrefix + "").ToString();
			return Post<UserRequest, UserResponse>(query, actor);
		}

		/// <summary>
		/// Update an existing User.
		/// </summary>
		/// <param name="id">Id of the existing User.</param>
		/// <param name="user"><see cref="UserRequest"/> object that holds the details of the User.</param>
		public void Update(int id, UserRequest user)
		{
			var query = GetUriBuilder(ControllerPrefix + "/update/{0}", id).ToString();
			Put(query, user);
		}

		/// <summary>
		/// Delete User with the <param name="id"/> provided.
		/// </summary>
		/// <param name="id">User ID.</param>
		public void Delete(int id)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}", id).ToString();
			Delete(query);
		}
	}
}
