using System;
using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Core.Controllers
{
    public abstract class ActorController
    {
        public static event Action<Actor> ActorUpdatedEvent;
        public static event Action<int> ActorDeletedEvent;

        protected void TriggerUpdatedEvent(Actor actor)
        {
            ActorUpdatedEvent?.Invoke(actor);
        }

        protected void TriggerDeletedEvent(int actorId)
        {
            ActorDeletedEvent?.Invoke(actorId);
        }
    }
}
