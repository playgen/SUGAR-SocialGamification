using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Account specific operations.
	/// </summary>
	public class AccountClient : ClientBase
	{
		private const string ControllerPrefix = "api/account";

		public AccountClient(
			string baseAddress, 
			IHttpHandler httpHandler, 
			Dictionary<string, string> persistentHeaders,
			AsyncRequestController asyncRequestController, 
			EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, persistentHeaders, asyncRequestController, evaluationNotifications)
		{
		}

		/// <summary>
		/// Register a new account and creates an associated user.
		/// Requires the <see cref="AccountRequest.Name"/> to be unique.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// </summary>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the details of the new Account.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the new Account details.</returns>
		public AccountResponse Create(AccountRequest accountRequest)
		{
			var query = GetUriBuilder(ControllerPrefix + "/create").ToString();
			return Post<AccountRequest, AccountResponse>(query, accountRequest);
		}

		public void CreateAsync(AccountRequest accountRequest, Action<AccountResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => Create(accountRequest),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Delete Accounts with the ID provided.
		/// </summary>
		/// <param name="id">Account ID.</param>
		public void Delete(int id)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}", id).ToString();
			Delete(query);
		}
	}
}
