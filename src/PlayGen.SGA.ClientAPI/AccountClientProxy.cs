using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    /// <summary>
    /// Controller that facilitates Account specific operations.
    /// </summary>
    public class AccountClientProxy : ClientProxy, IAccountController
    {
        public AccountClientProxy(string baseAddress) : base(baseAddress)
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
            var query = GetUriBuilder("api/account").ToString();
            return Put<AccountRequest, AccountResponse>(query, account);
        }

        /// <summary>
        /// Register a new account and creates an associated user.
        /// Requires the <see cref="AccountRequest.Name"/> to be unique.
        /// Returns a JsonWebToken used for authorization in any further calls to the API.
        /// </summary>
        /// <param name="newAccount"><see cref="AccountRequest"/> object that contains the details of the new Account.</param>
        /// <returns>A <see cref="AccountResponse"/> containing the new Account details.</returns>
        public AccountResponse Register(AccountRequest newAccount)
        {
            var query = GetUriBuilder("api/account").ToString();
            return Post<AccountRequest, AccountResponse>(query, newAccount);
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
            var query = GetUriBuilder("api/account/userId")
            .AppendQueryParameters(new int[] { userId }, "userId={0}")
            .ToString();
            return Post<AccountRequest, AccountResponse>(query, newAccount);
        }

        /// <summary>
        /// Delete Accounts with the IDs provided.
        /// </summary>
        /// <param name="id">Array of Account IDs.</param>
        public void Delete(int[] id)
        {
            var query = GetUriBuilder("api/account")
                .AppendQueryParameters(id, "id={0}")
                .ToString();
            Delete(query);
        }
    }
}
