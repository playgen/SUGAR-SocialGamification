using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGroupMemberController
    {
        void CreateMemberRequest(int userId, int groupId);

        IEnumerable<Actor> GetMemberRequests(int groupId);

        void UpdateMemberRequest(int userId, Relationship relationship);

        IEnumerable<Actor> GetMembers(int groupId);

        void UpdateMember(int userId, Relationship relationship);
    }
}