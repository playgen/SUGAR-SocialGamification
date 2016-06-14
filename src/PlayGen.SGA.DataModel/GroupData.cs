using PlayGen.SGA.DataModel.Interfaces;
using System;
using System.Collections.Generic;

namespace PlayGen.SGA.DataModel
{
    public class GroupData : IRecord, IModificationHistory
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
