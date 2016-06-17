using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;


namespace PlayGen.SGA.ClientAPI
{
    public class AccountClientProxy : ClientProxy, IAccountController
    {
        public void Register(string name, string password)
        {
            var query = GetUriBuilder("api/account").ToString();
            return Post<Game, int>(query, game);
        }

        public string Login(string name, string password)
        {
            throw new NotImplementedException();
        }

        public void Delete(string[] id)
        {
            throw new NotImplementedException();
        }
    }
}
