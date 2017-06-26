using System;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class ActorController
	{
		public static event Action<int> ActorDeletedEvent;

		private readonly Data.EntityFramework.Controllers.ActorController _actorDbController;

		public ActorController(Data.EntityFramework.Controllers.ActorController actorDbController)
		{
			_actorDbController = actorDbController;
		}

		protected void TriggerDeletedEvent(int actorId)
		{
			ActorDeletedEvent?.Invoke(actorId);
		}

		public Actor Get(int actorId)
		{
			return _actorDbController.Get(actorId);
		}
	}
}
