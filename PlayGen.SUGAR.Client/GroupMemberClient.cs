using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates User to Group relationship specific operations.
	/// </summary>
	public class GroupMemberClient : ClientBase
	{
		private const string ControllerPrefix = "api/groupmember";

		public GroupMemberClient(string baseAddress, IHttpHandler httpHandler, AsyncRequestController asyncRequestController, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, asyncRequestController, evaluationNotifications)
		{
		}

		/// <summary>
		/// Get a list of all Users that have relationship requests for this <param name="groupId"/>.
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> GetMemberRequests(int groupId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/requests/{0}", groupId).ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		public void GetMemberRequestsAsync(int userId, Action<IEnumerable<ActorResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetMemberRequests(userId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Get a list of all Groups that have been sent relationship requests for this <param name="userId"/>.
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
		/// Get a count of users that have a relationship with this <param name="groupId"/>.
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A count of members in the group that matches the search criteria.</returns>
		public int GetMemberCount(int groupId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/membercount/{0}", groupId).ToString();
			return Get<int>(query);
		}

		public void GetMemberCountAsync(int groupId, Action<int> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetMemberCount(groupId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Get a list of all Users that have relationships with this <param name="groupId"/>.
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> GetMembers(int groupId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/members/{0}", groupId).ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		public void GetMembersAsync(int groupId, Action<IEnumerable<ActorResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetMembers(groupId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Get a list of all Groups that have relationships with this <param name="userId"/>.
		/// </summary>
		/// <param name="userId">ID of the User.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> GetUserGroups(int userId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/usergroups/{0}", userId).ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		public void GetUserGroupsAsync(int userId, Action<IEnumerable<ActorResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetUserGroups(userId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Create a new relationship request between the User and Group.
		/// Requires a relationship between the User and Group to not already exist.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipRequest"/> object that holds the details of the new relationship request.</param>
		/// <returns>A <see cref="RelationshipResponse"/> containing the new Relationship details.</returns>
		public RelationshipResponse CreateMemberRequest(RelationshipRequest relationship)
		{
			var query = GetUriBuilder(ControllerPrefix + "").ToString();
			return Post<RelationshipRequest, RelationshipResponse>(query, relationship);
		}

		public void CreateMemberRequestAsync(RelationshipRequest relationship, Action<RelationshipResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => CreateMemberRequest(relationship),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Update an existing relationship request between <param name="relationship.UserId"/> and <param name="relationship.GroupId"/>.
		/// Requires the relationship request to already exist between the User and Group.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		public void UpdateMemberRequest(RelationshipStatusUpdate relationship)
		{
			var query = GetUriBuilder(ControllerPrefix + "/request").ToString();
			Put(query, relationship);
		}

		public void UpdateMemberRequestAsync(RelationshipStatusUpdate relationship, Action onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => UpdateMemberRequest(relationship),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Update an existing relationship between <param name="relationship.UserId"/> and <param name="relationship.GroupId"/>.
		/// Requires the relationship to already exist between the User and Group.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		public void UpdateMember(RelationshipStatusUpdate relationship)
		{
			var query = GetUriBuilder(ControllerPrefix + "").ToString();
			Put(query, relationship);
		}

		public void UpdateMemberAsync(RelationshipStatusUpdate relationship, Action onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => UpdateMember(relationship),
				onSuccess,
				onError);
		}
	}
}
