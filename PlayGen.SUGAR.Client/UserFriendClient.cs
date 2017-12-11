using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates User to User relationship specific operations.
	/// </summary>
	public class UserFriendClient : ClientBase
	{
		private const string ControllerPrefix = "api/userfriend";

		public UserFriendClient(
			string baseAddress,
			IHttpHandler httpHandler,
			Dictionary<string, string> persistentHeaders,
			AsyncRequestController asyncRequestController,
			EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, persistentHeaders, asyncRequestController, evaluationNotifications)
		{
		}

		/// <summary>
		/// Get a list of all Users that have relationship requests for this <param name="userId"/>.
		/// </summary>
		/// <param name="userId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> GetFriendRequests(int userId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/requests/{0}", userId).ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		public void GetFriendRequestsAsync(int userId, Action<IEnumerable<ActorResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetFriendRequests(userId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Get a list of all Users that have been sent relationship requests for this <param name="userId"/>.
		/// </summary>
		/// <param name="userId">ID of the user.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> GetSentRequests(int userId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/sentrequests/{0}", userId).ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		public void GetSentRequestsAsync(int userId, Action<IEnumerable<ActorResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetSentRequests(userId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Get a list of all Users that have relationships with this <param name="userId"/>.
		/// </summary>
		/// <param name="userId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> GetFriends(int userId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/friends/{0}", userId).ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		public void GetFriendsAsync(int userId, Action<IEnumerable<ActorResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetFriends(userId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Create a new relationship request between two Users.
		/// Requires a relationship between the two to not already exist.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipRequest"/> object that holds the details of the new relationship request.</param>
		/// <returns>A <see cref="RelationshipResponse"/> containing the new Relationship details.</returns>
		public RelationshipResponse CreateFriendRequest(RelationshipRequest relationship)
		{
			var query = GetUriBuilder(ControllerPrefix + "").ToString();
			return Post<RelationshipRequest, RelationshipResponse>(query, relationship);
		}

		public void CreateFriendRequestAsync(RelationshipRequest relationship, Action<RelationshipResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => CreateFriendRequest(relationship),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Update an existing relationship request between <param name="relationship.RequestorId"/> and <param name="relationship.AcceptorId"/>.
		/// Requires the relationship request to already exist between the two Users.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		public void UpdateFriendRequest(RelationshipStatusUpdate relationship)
		{
			var query = GetUriBuilder(ControllerPrefix + "/request").ToString();
			Put(query, relationship);
		}

		public void UpdateFriendRequestAsync(RelationshipStatusUpdate relationship, Action onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => UpdateFriendRequest(relationship),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Update an existing relationship between <param name="relationship.RequestorId"/> and <param name="relationship.AcceptorId"/>.
		/// Requires the relationship to already exist between the two Users.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		public void UpdateFriend(RelationshipStatusUpdate relationship)
		{
			var query = GetUriBuilder(ControllerPrefix + "").ToString();
			Put(query, relationship);
		}

		public void UpdateFriendAsync(RelationshipStatusUpdate relationship, Action onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => UpdateFriend(relationship),
				onSuccess,
				onError);
		}
	}
}
