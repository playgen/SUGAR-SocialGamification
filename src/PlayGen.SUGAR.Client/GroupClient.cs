using System;
using System.Collections.Generic;
using System.Net;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	///     Controller that facilitates Group specific operations.
	/// </summary>
	public class GroupClient : ClientBase
	{
		private const string ControllerPrefix = "api/group";

		public GroupClient(string baseAddress, IHttpHandler httpHandler, AsyncRequestController asyncRequestController,
			EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, asyncRequestController, evaluationNotifications)
		{
		}

		/// <summary>
		///     Get a list of all Groups.
		/// </summary>
		/// <returns>A list of <see cref="GroupResponse" /> that hold Group details.</returns>
		public GroupsResponse Get()
		{
			var query = GetUriBuilder(ControllerPrefix + "/list").ToString();
			return Get<GroupsResponse>(query);
		}

		/// <summary>
		///     Get a list of Groups that match
		///     <param name="name" />
		///     provided.
		/// </summary>
		/// <param name="name">Group name.</param>
		/// <returns>A list of <see cref="GroupResponse" /> which match the search criteria.</returns>
		public GroupsResponse Get(string name)
		{
			var query = GetUriBuilder(ControllerPrefix + "/find/{0}", name)
				.ToString();
			return Get<GroupsResponse>(query);
		}

		public void GetAsync(string name, Action<GroupsResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => Get(name),
				onSuccess,
				onError);
		}

		/// <summary>
		///     Get Group that matches
		///     <param name="id" />
		///     provided.
		/// </summary>
		/// <param name="id">Group id.</param>
		/// <returns><see cref="GroupResponse" /> which matches search criteria.</returns>
		public GroupResponse Get(int id)
		{
			var query = GetUriBuilder(ControllerPrefix + "/findbyid/{0}", id)
				.ToString();
			return Get<GroupResponse>(query, new[] {HttpStatusCode.OK, HttpStatusCode.NoContent});
		}

		/// <summary>
		///     Create a new Group.
		///     Requires the <see cref="GroupRequest" />'s Name to be unique for Groups.
		/// </summary>
		/// <param name="actor"><see cref="GroupRequest" /> object that holds the details of the new Group.</param>
		/// <returns>A <see cref="GroupResponse" /> containing the new Group details.</returns>
		public GroupResponse Create(GroupRequest actor)
		{
			var query = GetUriBuilder(ControllerPrefix + "")
				.ToString();
			return Post<GroupRequest, GroupResponse>(query, actor);
		}

		/// <summary>
		///     Update an existing Group.
		/// </summary>
		/// <param name="id">Id of the existing Group.</param>
		/// <param name="group"><see cref="GroupRequest" /> object that holds the details of the Group.</param>
		public void Update(int id, GroupRequest group)
		{
			var query = GetUriBuilder(ControllerPrefix + "/update/{0}", id)
				.ToString();
			Put(query, group);
		}

		/// <summary>
		///     Delete group with the
		///     <param name="id" />
		///     provided.
		/// </summary>
		/// <param name="id">Group ID.</param>
		public void Delete(int id)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}", id)
				.ToString();
			Delete(query);
		}
	}
}