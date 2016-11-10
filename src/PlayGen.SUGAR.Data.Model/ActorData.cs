using System;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
    public class ActorData : IModificationHistory
    {
        public int Id { get; set; }

        public int? ActorId { get; set; }

        public string Key { get; set; }

        public virtual string Value { get; set; }

        public virtual GameDataType DataType { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}