using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
{
    public class UserAchievement : IRecord
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public string Name { get; set; }

        public string CompletionCondition { get; set; }
    }
}
