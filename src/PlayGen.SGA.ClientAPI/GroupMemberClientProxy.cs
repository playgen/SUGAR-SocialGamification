using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    public class GroupMemberClientProxy : ClientProxy, IGroupMemberController
    {
        public void CreateMemberRequest(Relationship relationship)
        {
            var query = GetUriBuilder("api/groupmember").ToString();
            Post<Relationship, int>(query, relationship);
        }

        public IEnumerable<Actor> GetMemberRequests(int groupId)
        {
            var query = GetUriBuilder("api/groupmember/requests")
                .AppendQueryParameters(new int[] {groupId}, "groupId={0}")
                .ToString();
            return Get<IEnumerable<Actor>>(query);
        }

        public void UpdateMemberRequest(Relationship relationship)
        {
            var query = GetUriBuilder("api/groupmember/request").ToString();
            Put<Relationship, int>(query, relationship);
        }

        public IEnumerable<Actor> GetMembers(int groupId)
        {
            var query = GetUriBuilder("api/groupmember/members")
                .AppendQueryParameters(new int[] { groupId }, "groupId={0}")
                .ToString();
            return Get<IEnumerable<Actor>>(query);
        }

        public void UpdateMember(Relationship relationship)
        {
            var query = GetUriBuilder("api/groupmember").ToString();
            Put<Relationship, int>(query, relationship);
        }
    }
}
