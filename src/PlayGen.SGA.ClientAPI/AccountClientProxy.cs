using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;


namespace PlayGen.SGA.ClientAPI
{
    public class AccountClientProxy : ClientProxy, IAccountController
    {
        public int Register(Account newAccount)
        {
            var query = GetUriBuilder("api/account").ToString();
            return Post<Account, int>(query, newAccount);
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

        public int Register(AccountResponse newAccount)
        {
            throw new NotImplementedException();
        }

        public new void Delete(int[] id)
        {
            throw new NotImplementedException();
        }
    }
}
