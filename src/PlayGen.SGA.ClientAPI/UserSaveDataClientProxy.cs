using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    public class UserSaveDataClientProxy : ClientProxy, IUserSaveDataController
    {
        public UserSaveDataClientProxy(string baseAddress) : base(baseAddress)
        {
        }

        public IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] keys)
        {
            var query = GetUriBuilder("api/game/usersavedata")
                .AppendQueryParameters(new int[] { actorId }, "actorId={0}")
                .AppendQueryParameters(new int[] { gameId }, "gameId={0}")
                .AppendQueryParameters(keys, "key={0}")
                .ToString();
            return Get<IEnumerable<SaveDataResponse>>(query);
        }

        public SaveDataResponse Add(SaveDataRequest data)
        {
            var query = GetUriBuilder("api/userfriend").ToString();
            return Post<SaveDataRequest, SaveDataResponse>(query, data);
        }
    }
}
