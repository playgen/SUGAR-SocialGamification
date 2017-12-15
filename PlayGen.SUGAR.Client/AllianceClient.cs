using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.RequestQueue;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates User to Group relationship specific operations.
	/// </summary>
	public class AllianceClient : ClientBase
	{
		private const string ControllerPrefix = "api/alliance";

		public AllianceClient(
			string baseAddress,
			IHttpHandler httpHandler,
			Dictionary<string, string> persistentHeaders,
			Dictionary<string, string> sessionHeaders,
			IRequestQueue requestQueue,
			EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, persistentHeaders, sessionHeaders, requestQueue, evaluationNotifications)
		{
		}

		/// <summary>
		/// Get a list of all groups that have relationship requests for this <param name="groupId"/>.
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> GetAllianceRequests(int groupId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/requests/{0}", groupId).ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		public void GetAllianceRequestsAsync(int groupId, Action<IEnumerable<ActorResponse>> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => GetAllianceRequests(groupId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Get a list of all Groups that have been sent relationship requests for this <param name="groupId"/>.
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> GetSentRequests(int groupId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/sentrequests/{0}", groupId).ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		public void GetSentRequestsAsync(int groupId, Action<IEnumerable<ActorResponse>> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => GetSentRequests(groupId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Get a count of groups that have a relationship with this <param name="groupId"/>.
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A count of members in the group that matches the search criteria.</returns>
		public int GetAllianceCount(int groupId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/alliancecount/{0}", groupId).ToString();
			return Get<int>(query);
		}

		public void GetAllianceCountAsync(int groupId, Action<int> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => GetAllianceCount(groupId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Get a list of all Groups that have relationships with this <param name="groupId"/>.
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> GetAlliances(int groupId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}", groupId).ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		public void GetAlliancesAsync(int groupId, Action<IEnumerable<ActorResponse>> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => GetAlliances(groupId),
				onSuccess,
				onError);
		}
		
		/// <summary>
		/// Create a new relationship request between the two groups.
		/// Requires a relationship between two groups to not already exist.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipRequest"/> object that holds the details of the new relationship request.</param>
		/// <returns>A <see cref="RelationshipResponse"/> containing the new Relationship details.</returns>
		public RelationshipResponse CreateAllianceRequest(RelationshipRequest relationship)
		{
			var query = GetUriBuilder(ControllerPrefix).ToString();
			return Post<RelationshipRequest, RelationshipResponse>(query, relationship);
		}

		public void CreateAllianceRequestAsync(RelationshipRequest relationship, Action<RelationshipResponse> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => CreateAllianceRequest(relationship),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Update an existing relationship request between two groups.
		/// Requires the relationship request to already exist between the two Groups.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		public void UpdateAllianceRequest(RelationshipStatusUpdate relationship)
		{
			var query = GetUriBuilder(ControllerPrefix + "/request").ToString();
			Put(query, relationship);
		}

		public void UpdateAllianceRequestAsync(RelationshipStatusUpdate relationship, Action onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => UpdateAllianceRequest(relationship),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Update an existing relationship between two groups.
		/// Requires the relationship to already exist between the two groups.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		public void UpdateAlliance(RelationshipStatusUpdate relationship)
		{
			var query = GetUriBuilder(ControllerPrefix).ToString();
			Put(query, relationship);
		}

		public void UpdateAllianceAsync(RelationshipStatusUpdate relationship, Action onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => UpdateAlliance(relationship),
				onSuccess,
				onError);
		}
	}
}
