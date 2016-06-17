using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGroupMemberController
    {
        RelationshipResponse CreateMemberRequest(RelationshipRequest relationship);

        IEnumerable<ActorResponse> GetMemberRequests(int groupId);

        void UpdateMemberRequest(RelationshipStatusUpdate relationship);

        IEnumerable<ActorResponse> GetMembers(int groupId);

        void UpdateMember(RelationshipStatusUpdate relationship);
    }
}