using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    /// <summary>
    /// Controller that facilitates User to Group relationship specific operations.
    /// </summary>
    public class GroupMemberClientProxy : ClientProxyBase, IGroupMemberController
    {
        public GroupMemberClientProxy(string baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Get a list of all Users that have relationship requests for this <param name="groupId"/>.
        /// </summary>
        /// <param name="groupId">ID of the group.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        public IEnumerable<ActorResponse> GetMemberRequests(int groupId)
        {
            var query = GetUriBuilder("api/groupmember/requests")
                .AppendQueryParameters(new int[] { groupId }, "groupId={0}")
                .ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        /// <summary>
        /// Get a list of all Users that have relationships with this <param name="groupId"/>.
        /// </summary>
        /// <param name="groupId">ID of the group.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        public IEnumerable<ActorResponse> GetMembers(int groupId)
        {
            var query = GetUriBuilder("api/groupmember/members")
                .AppendQueryParameters(new int[] { groupId }, "groupId={0}")
                .ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        /// <summary>
        /// Create a new relationship request between the User and Group.
        /// Requires a relationship between the User and Group to not already exist.
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipRequest"/> object that holds the details of the new relationship request.</param>
        /// <returns>A <see cref="RelationshipResponse"/> containing the new Relationship details.</returns>
        public RelationshipResponse CreateMemberRequest(RelationshipRequest relationship)
        {
            var query = GetUriBuilder("api/groupmember").ToString();
            return Post<RelationshipRequest, RelationshipResponse>(query, relationship);
        }

        /// <summary>
        /// Update an existing relationship request between <param name="relationship.UserId"/> and <param name="relationship.GroupId"/>.
        /// Requires the relationship request to already exist between the User and Group.
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
        public void UpdateMemberRequest(RelationshipStatusUpdate relationship)
        {
            var query = GetUriBuilder("api/groupmember/request").ToString();
            Put(query, relationship);
        }

        /// <summary>
        /// Update an existing relationship between <param name="relationship.UserId"/> and <param name="relationship.GroupId"/>.
        /// Requires the relationship to already exist between the User and Group.
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
        public void UpdateMember(RelationshipStatusUpdate relationship)
        {
            var query = GetUriBuilder("api/groupmember").ToString();
            Put(query, relationship);
        }
    }
}
