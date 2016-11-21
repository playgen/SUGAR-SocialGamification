﻿using System;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Account specific operations.
	/// </summary>
	public class AccountClient : ClientBase
	{
		public AccountClient(string baseAddress, IHttpHandler httpHandler, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, evaluationNotifications)
		{
		}

		/// <summary>
		/// Logs in an account based on the name and password combination.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// </summary>
		/// <param name="account"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		public AccountResponse Login(AccountRequest account)
		{
			Console.WriteLine("AccountClient::Login");
			var query = GetUriBuilder("api/account/login").ToString();
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
			Console.WriteLine("AccountClient::Register");
			var query = GetUriBuilder("api/account/register").ToString();
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
		public AccountResponse Register(int userId, AccountRequest newAccount)
		{
			var query = GetUriBuilder("api/account/registerwithid/{0}", userId).ToString();
			return Post<AccountRequest, AccountResponse>(query, newAccount);
		}

		/// <summary>
		/// Delete Accounts with the ID provided.
		/// </summary>
		/// <param name="id">Account ID.</param>
		public void Delete(int id)
		{
			var query = GetUriBuilder("api/account/{0}", id).ToString();
			Delete(query);
		}
	}
}
