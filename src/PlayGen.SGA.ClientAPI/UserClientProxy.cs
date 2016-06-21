using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    /// <summary>
    /// Controller that facilitates User specific operations.
    /// </summary>
    public class UserClientProxy : ClientProxy, IUserController
    {
        public UserClientProxy(string baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Get a list of all Users.
        /// </summary>
        /// <returns>A list of <see cref="ActorResponse"/> that hold User details.</returns>
        public IEnumerable<ActorResponse> Get()
        {
            var query = GetUriBuilder("api/game/user").ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        /// <summary>
        /// Get a list of Users that match <param name="name"/> provided.
        /// </summary>
        /// <param name="name">Array of User names.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        public IEnumerable<ActorResponse> Get(string[] name)
        {
            var query = GetUriBuilder("api/user")
                .AppendQueryParameters(name, "name={0}")
                .ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        /// <summary>
        /// Create a new User.
        /// Requires the <see cref="ActorRequest.Name"/> to be unique for Users.
        /// </summary>
        /// <param name="actor"><see cref="ActorRequest"/> object that holds the details of the new User.</param>
        /// <returns>A <see cref="ActorResponse"/> containing the new User details.</returns>
        public ActorResponse Create(ActorRequest actor)
        {
            var query = GetUriBuilder("api/user").ToString();
            return Post<ActorRequest, ActorResponse>(query, actor);
        }

        /// <summary>
        /// Delete Users with the <param name="id"/> provided.
        /// </summary>
        /// <param name="id">Array of User IDs.</param>
        public void Delete(int[] id)
        {
            var query = GetUriBuilder("api/user")
                .AppendQueryParameters(id, "id={0}")
                .ToString();
            Delete(query);
        }
    }
}
