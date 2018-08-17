using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class ActorController
	{
		public static event Action<int> ActorDeletedEvent;

		private readonly EntityFramework.Controllers.ActorController _actorDbController;
		private readonly ActorRoleController _actorRoleController;

		public ActorController(EntityFramework.Controllers.ActorController actorDbController, ActorRoleController actorRoleController)
		{
			_actorDbController = actorDbController;
			_actorRoleController = actorRoleController;
		}

		protected void TriggerDeletedEvent(int actorId)
		{
			ActorDeletedEvent?.Invoke(actorId);
		}

		public Actor Get(int actorId)
		{
			return _actorDbController.Get(actorId);
		}

		protected List<T> FilterPrivate<T>(List<T> actors, int requestingId) where T : Actor
		{
			var includePrivate = _actorRoleController.GetControlled(requestingId).Any(c => c.ClaimScope == ClaimScope.Global);
			if (!includePrivate)
			{
				return actors.Where(a => !a.Private || a.Id == requestingId).ToList();
			}
			return actors;
		}

		protected T FilterPrivate<T>(T actor, int requestingId) where T : Actor
		{
			var includePrivate = _actorRoleController.GetControlled(requestingId).Any(c => c.ClaimScope == ClaimScope.Global);
			if (!includePrivate && actor.Private && actor.Id != requestingId)
			{
				return null;
			}
			return actor;
		}
	}
}

