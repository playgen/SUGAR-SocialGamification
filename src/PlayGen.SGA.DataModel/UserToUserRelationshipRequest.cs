using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
{
    public class UserToUserRelationshipRequest : IRecord, IRelationship
    {
        public int Id { get; set; }

        public int RequestorId { get; set; }

        public int AcceptorId { get; set; }
    }
}