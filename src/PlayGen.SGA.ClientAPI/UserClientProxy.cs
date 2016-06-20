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
        public UserClientProxy(string baseAddress) : base(baseAddress)
        {
        }

        public IEnumerable<ActorResponse> Get()
        {
            var query = GetUriBuilder("api/game/user").ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        public IEnumerable<ActorResponse> Get(string[] name)
        {
            var query = GetUriBuilder("api/user")
                .AppendQueryParameters(name, "name={0}")
                .ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        public ActorResponse Create(ActorRequest actor)
        {
            var query = GetUriBuilder("api/user").ToString();
            return Post<ActorRequest, ActorResponse>(query, actor);
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
