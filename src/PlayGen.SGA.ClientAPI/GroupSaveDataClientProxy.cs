using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;


namespace PlayGen.SGA.ClientAPI
{
    public class GroupSaveDataClientProxy : ClientProxy, IGroupSaveDataController
    {
        public SaveDataResponse Add(SaveDataRequest data)
        {
            var query = GetUriBuilder("api/groupsavedata").ToString();
            return Post<SaveDataRequest, SaveDataResponse>(query, data);
        }

        public IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] keys)
        {
            var query = GetUriBuilder("api/game/groupsavedata")
                .AppendQueryParameters(new int[] { actorId }, "actorId={0}")
                .AppendQueryParameters(new int[] { gameId }, "gameId={0}")
                .AppendQueryParameters(keys, "key={0}")
                .ToString();
            return Get<IEnumerable<SaveDataResponse>>(query);
        }
    }
}
