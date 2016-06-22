using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGroupMemberController
    {
        IEnumerable<ActorResponse> GetMemberRequests(int groupId);

        IEnumerable<ActorResponse> GetMembers(int groupId);

        IEnumerable<ActorResponse> GetUserGroups(int userId);

        RelationshipResponse CreateMemberRequest(RelationshipRequest relationship);

        void UpdateMemberRequest(RelationshipStatusUpdate relationship);

        void UpdateMember(RelationshipStatusUpdate relationship);
    }
}