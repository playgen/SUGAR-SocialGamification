using System.Collections.Concurrent;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
	/// <summary>
	///     Check newly evaluated progress and store notifications for values of any significance
	/// </summary>
	public class ProgressNotificationCache
	{
		// <gameId, <actorId, <evaluation, progress>>>
		// Latest stored last in <evaluation, progress> list.
		private readonly ConcurrentProgressCache _pendingNotifications = new ConcurrentProgressCache();

		public bool Remove(int? gameId, int actorId)
		{
			return _pendingNotifications.RemoveActor(gameId, actorId);
		}

		public bool Remove(int evaluationId)
		{
			return _pendingNotifications.Remove(evaluationId);
		}

		public void
			Update(ConcurrentProgressCache addProgress,
				float minProgress =
					1f) // todo make this config driven (should probably be stored on the achievemnt as notification progress interval)
		{
			_pendingNotifications.AddProgress(addProgress, progress => progress >= minProgress);
		}

		public ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> Get(int? gameId, int actorId)
		{
			var actorsProgress = _pendingNotifications.TakeActorProgress(gameId, actorId);

			// todo also add progress updates for groups here
			return actorsProgress;
		}
	}
}