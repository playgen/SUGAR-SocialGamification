using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;


namespace PlayGen.SGA.ClientAPI
{
    public class AccountClientProxy : ClientProxy, IAccountController
    {
        public AccountResponse Register(AccountRequest newAccount)
        {
            //var query = GetUriBuilder("api/account").ToString();
            //return Post<Game, int>(query, game);
            throw new NotImplementedException();
        }

        public AccountResponse Login(AccountRequest account)
        {
            throw new NotImplementedException();
        }

        public void Delete(string[] id)
        {
            throw new NotImplementedException();
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
