using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    /// <summary>
    /// Controller that facilitates Group specific operations.
    /// </summary>
    public class GroupClientProxy : ClientProxyBase, IGroupController
    {
        public GroupClientProxy(string baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Get a list of all Groups.
        /// </summary>
        /// <returns>A list of <see cref="ActorResponse"/> that hold Group details.</returns>
        public IEnumerable<ActorResponse> Get()
        {
            var query = GetUriBuilder("api/group/all").ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        /// <summary>
        /// Get a list of Groups that match <param name="name"/> provided.
        /// </summary>
        /// <param name="name">Array of group names.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        public IEnumerable<ActorResponse> Get(string[] name)
        {
            var query = GetUriBuilder("api/group")
                .AppendQueryParameters(name, "name={0}")
                .ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        /// <summary>
        /// Create a new Group.
        /// Requires the <see cref="ActorRequest.Name"/> to be unique for Groups.
        /// </summary>
        /// <param name="actor"><see cref="ActorRequest"/> object that holds the details of the new Group.</param>
        /// <returns>A <see cref="ActorResponse"/> containing the new Group details.</returns>
        public ActorResponse Create(ActorRequest actor)
        {
            var query = GetUriBuilder("api/group").ToString();
            return Post<ActorRequest, ActorResponse>(query, actor);
        }

        /// <summary>
        /// Delete groups with the <param name="id"/> provided.
        /// </summary>
        /// <param name="id">Array of Group IDs.</param>
        public void Delete(int[] id)
        {
            var query = GetUriBuilder("api/game")
                .AppendQueryParameters(id, "id={0}")
                .ToString();
            Delete(query);
        }
    }
}
