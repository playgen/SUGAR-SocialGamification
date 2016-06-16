using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGroupMemberController
    {
        void CreateMemberRequest(Relationship relationship);

        IEnumerable<Actor> GetMemberRequests(int groupId);

        void UpdateMemberRequest(Relationship relationship);

        IEnumerable<Actor> GetMembers(int groupId);

        void UpdateMember(Relationship relationship);
    }
}