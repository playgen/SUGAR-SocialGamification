using System;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Core.Extensions;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class ActorController
	{
		public static event Action<int> DeleteActorEvent;

		private readonly EntityFramework.Controllers.ActorController _actorDbController;
		
		public ActorController(EntityFramework.Controllers.ActorController actorDbController)
		{
			_actorDbController = actorDbController;
		}

		protected void TriggerDeleteEvent(int actorId)
		{
			DeleteActorEvent?.Invoke(actorId);
		}

		public Actor Get(int actorId, ActorVisibilityFilter actorVisibilityFilter = ActorVisibilityFilter.Public)
		{
			var actor = _actorDbController.Get(actorId);
			return actor.FilterVisibility(actorVisibilityFilter);
		}
	}
}

