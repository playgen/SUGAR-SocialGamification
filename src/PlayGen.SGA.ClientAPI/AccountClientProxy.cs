using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;


namespace PlayGen.SGA.ClientAPI
{
    public class AccountClientProxy : ClientProxyBase, IAccountController
    {
        public AccountClientProxy(string baseAddress) : base(baseAddress)
        {
        }

        public AccountResponse Register(AccountRequest newAccount)
        {
            var query = GetUriBuilder("api/account").ToString();
            return Post<AccountRequest, AccountResponse>(query, newAccount);
        }

        public AccountResponse Login(AccountRequest account)
        {
            //TODO: Add login call
            throw new NotImplementedException();
        }

        public void Delete(int[] id)
        {
            var query = GetUriBuilder("api/account")
                .AppendQueryParameters(id, "id={0}")
                .ToString();
            Delete(query);
        }

        public AccountResponse Register(int userId, AccountRequest newAccount)
        {
            throw new NotImplementedException();
        }
    }
}
