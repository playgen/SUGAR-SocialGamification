using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Resource specific operations.
	/// </summary>
	public class ResourceClient : ClientBase
	{
		private const string ControllerPrefix = "api/resource";

		public ResourceClient(string baseAddress, IHttpHandler httpHandler, AsyncRequestController asyncRequestController, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, asyncRequestController, evaluationNotifications)
		{
		}

		/// <summary>
		/// Find a list of all Resources that match the <param name="gameId"/>, <param name="actorId"/> and <param name="keys"/> provided.
		/// </summary>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="keys">Array of Key names.</param>
		/// <returns>A list of <see cref="ResourceResponse"/> which match the search criteria.</returns>
		public IEnumerable<ResourceResponse> Get(int? gameId, int? actorId, string[] keys)
		{
			var query = GetUriBuilder(ControllerPrefix)
				.AppendQueryParameter(gameId, "gameId={0}")
				.AppendQueryParameter(actorId, "actorId={0}")
				.AppendQueryParameters(keys, "keys={0}")
				.ToString();
			return Get<IEnumerable<ResourceResponse>>(query);
		}

		public void GetAsync(int? gameId, int? actorId, string[] keys, Action<IEnumerable<ResourceResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => Get(gameId, actorId, keys),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Create or Updates a Resource record.
		/// </summary>
		/// <param name="data"><see cref="ResourceAddRequest"/> object that holds the details of the new Resource.</param>
		/// <returns>A <see cref="ResourceResponse"/> containing the new Resource details.</returns>
		public ResourceResponse AddOrUpdate(ResourceAddRequest data)
		{
			var query = GetUriBuilder(ControllerPrefix).ToString();
			return Post<ResourceAddRequest, ResourceResponse>(query, data);
		}

		public void AddOrUpdateAsync(ResourceAddRequest data, Action<ResourceResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => AddOrUpdate(data),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Transfers a quantity of a specific resource.
		/// </summary>
		/// <param name="data"></param>
		/// <returns>A <see cref="ResourceTransferResponse"/> containing the modified resources.</returns>
		public ResourceTransferResponse Transfer(ResourceTransferRequest data)
		{
			var query = GetUriBuilder(ControllerPrefix + "/transfer").ToString();
			return Post<ResourceTransferRequest, ResourceTransferResponse>(query, data);
		}

		public void TransferAsync(ResourceTransferRequest data, Action<ResourceTransferResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => Transfer(data),
				onSuccess,
				onError);
		}
	}
}
