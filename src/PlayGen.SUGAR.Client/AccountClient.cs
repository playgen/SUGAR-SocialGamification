using System;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Account specific operations.
	/// </summary>
	public class AccountClient : ClientBase
	{
		private const string ControllerPrefix = "api/account";

		public AccountClient(string baseAddress, IHttpHandler httpHandler, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, evaluationNotifications)
		{
		}

		/// <summary>
		/// Logs in an account into the system based on the name and password combination.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// </summary>
		/// <param name="account"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		public AccountResponse Login(AccountRequest account)
		{
			var query = GetUriBuilder(ControllerPrefix + "/login").ToString();
			return Post<AccountRequest, AccountResponse>(query, account);
		}

		/// <summary>
		/// Logs in an account into a game based on the name and password combination.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// </summary>
		/// <param name="gameId">ID of the game the user is logging into.</param>
		/// <param name="account"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		public AccountResponse Login(int gameId, AccountRequest account)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/login", gameId).ToString();
			return Post<AccountRequest, AccountResponse>(query, account);
		}

		/// <summary>
		/// Register a new account and creates an associated user.
		/// Requires the <see cref="AccountRequest.Name"/> to be unique.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// </summary>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the details of the new Account.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the new Account details.</returns>
		public AccountResponse Register(AccountRequest accountRequest)
		{
			var query = GetUriBuilder(ControllerPrefix + "/register").ToString();
			return Post<AccountRequest, AccountResponse>(query, accountRequest);
		}

		/// <summary>
		/// Register a new account and creates an associated user.
		/// Requires the <see cref="AccountRequest.Name"/> to be unique.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// </summary>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the details of the new Account.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the new Account details.</returns>
		public AccountResponse Register(int gameId, AccountRequest accountRequest)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/register", gameId).ToString();
			return Post<AccountRequest, AccountResponse>(query, accountRequest);
		}

		/// <summary>
		/// Register a new account for an existing user.
		/// Requires the <see cref="AccountRequest.Name"/> to be unique.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// 
		/// </summary>
		/// <param name="userId">ID of the existing User.</param>
		/// <param name="newAccount"><see cref="AccountRequest"/> object that contains the details of the new Account.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the new Account details.</returns>
		public AccountResponse RegisterWithExisting(int userId, AccountRequest newAccount)
		{
			var query = GetUriBuilder(ControllerPrefix + "/registerwithid/{0}", userId).ToString();
			return Post<AccountRequest, AccountResponse>(query, newAccount);
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
