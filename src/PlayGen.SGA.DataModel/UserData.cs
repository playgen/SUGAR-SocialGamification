using System;
using System.Collections.Generic;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
{
    public class UserData : IRecord, IModificationHistory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
