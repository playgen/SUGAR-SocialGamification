using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;


namespace PlayGen.SGA.ClientAPI
{
    /// <summary>
    /// Controller that facilitates GroupData specific operations.
    /// </summary>
    public class GroupSaveDataClientProxy : ClientProxy, IGroupSaveDataController
    {
        /// <summary>
        /// Get a list of all GroupData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="keys"/> provided.
        /// </summary>
        /// <param name="actorId">ID of a Group.</param>
        /// <param name="gameId">ID of a Game.</param>
        /// <param name="keys">Array of Key names.</param>
        /// <returns>A list of <see cref="SaveDataResponse"/> which match the search criteria.</returns>
        public IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] keys)
        {
            var query = GetUriBuilder("api/game/groupsavedata")
                .AppendQueryParameters(new int[] { actorId }, "actorId={0}")
                .AppendQueryParameters(new int[] { gameId }, "gameId={0}")
                .AppendQueryParameters(keys, "key={0}")
                .ToString();
            return Get<IEnumerable<SaveDataResponse>>(query);
        }

        /// <summary>
        /// Create a new GroupData record.
        /// </summary>
        /// <param name="data"><see cref="SaveDataRequest"/> object that holds the details of the new GroupData.</param>
        /// <returns>A <see cref="SaveDataResponse"/> containing the new GroupData details.</returns>
        public SaveDataResponse Add(SaveDataRequest data)
        {
            var query = GetUriBuilder("api/groupsavedata").ToString();
            return Post<SaveDataRequest, SaveDataResponse>(query, data);
        }
    }
}
