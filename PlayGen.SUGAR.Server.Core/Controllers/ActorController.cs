using System;
using PlayGen.SUGAR.Server.Core.Extensions;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class ActorController
	{
		public static event Action<int> DeleteActorEvent;

		private readonly EntityFramework.Controllers.ActorController _actorDbController;
		private readonly ActorClaimController _actorClaimController;
		private EntityFramework.Controllers.ActorController actorDbController;


		public ActorController(EntityFramework.Controllers.ActorController actorDbController, ActorClaimController actorClaimController)
		{
			_actorDbController = actorDbController;
			_actorClaimController = actorClaimController;
		}

		protected void TriggerDeleteEvent(int actorId)
		{
			DeleteActorEvent?.Invoke(actorId);
		}

		public Actor Get(int actorId)
		{
			return _actorDbController.Get(actorId).FilterPrivate(_actorClaimController, null);
		}
	}
}

