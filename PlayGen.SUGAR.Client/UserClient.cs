using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates User specific operations.
	/// </summary>
	public class UserClient : ClientBase
	{
		private const string ControllerPrefix = "api/user";

		public UserClient(
			string baseAddress,
			IHttpHandler httpHandler,
			Dictionary<string, string> constantHeaders,
			Dictionary<string, string> sessionHeaders,
			AsyncRequestController asyncRequestController,
			EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, constantHeaders, sessionHeaders, asyncRequestController, evaluationNotifications)
		{
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

		public void GetAsync(string name, Action < IEnumerable<UserResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => Get(name),
				onSuccess,
				onError);
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

		public void GetAsync(int id, Action<UserResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => Get(id),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Update an existing User.
		/// </summary>
		/// <param name="id">Id of the existing User.</param>
		/// <param name="user"><see cref="UserRequest"/> object that holds the details of the User.</param>
		public UserResponse Update(int id, UserRequest user)
		{
			var query = GetUriBuilder(ControllerPrefix + "/update/{0}", id).ToString();
			return Put<UserRequest, UserResponse>(query, user);
		}

		public void UpdateAsync(int id, UserRequest user, Action onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => Update(id, user),
				onSuccess,
				onError);
		}
	}
}
