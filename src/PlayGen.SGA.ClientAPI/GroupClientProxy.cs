using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    public class GroupClientProxy : ClientProxy, IGroupController
    {
        public IEnumerable<ActorResponse> Get()
        {
            var query = GetUriBuilder("api/group/all").ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        public IEnumerable<ActorResponse> Get(string[] name)
        {
            var query = GetUriBuilder("api/group")
                .AppendQueryParameters(name, "name={0}")
                .ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }
        
        public ActorResponse Create(ActorRequest actor)
        {
            var query = GetUriBuilder("api/group").ToString();
            throw new NotImplementedException();
            //return Post<ActorResponse, int>(query, actor);
        }

        public void Delete(int[] id)
        {
            var query = GetUriBuilder("api/game")
                .AppendQueryParameters(id, "id={0}")
                .ToString();
            Delete(query);
        }
    }
}
