using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SGA.Contracts
{
    public class Achievement
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public string Name { get; set; }

        public string CompletionCondition { get; set; }
    }
}
