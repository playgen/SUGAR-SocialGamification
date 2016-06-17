using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    public class UserClientProxy : ClientProxy, IUserController
    {
        public int Create(Actor actor)
        {
            var query = GetUriBuilder("api/user").ToString();
            return Post<Actor, int>(query, actor);
        } 

        public IEnumerable<Actor> Get()
        {
            var query = GetUriBuilder("api/game/user").ToString();
            return Get<IEnumerable<Actor>>(query);
        }

        public IEnumerable<Actor> Get(string[] name)
        {
            var query = GetUriBuilder("api/user")
                .AppendQueryParameters(name, "name={0}")
                .ToString();
            return Get<IEnumerable<Actor>>(query);
        }

        public void Delete(int[] id)
        {
            var query = GetUriBuilder("api/user")
                .AppendQueryParameters(id, "id={0}")
                .ToString();
            Delete(query);
        }
    }
}
