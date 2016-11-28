using System;

namespace PlayGen.SUGAR.Core.Controllers
{
    public abstract class ActorController
    {
        public static event Action<int> ActorDeletedEvent;

        protected void TriggerDeletedEvent(int actorId)
        {
            ActorDeletedEvent?.Invoke(actorId);
        }
    }
}
