using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Contracts.Shared
{
    public abstract class Actor
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public abstract ActorType ActorType { get; }
    }
}
