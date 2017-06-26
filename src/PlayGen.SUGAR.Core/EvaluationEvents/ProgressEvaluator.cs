// todo v2.0: support group evaluations

using System.Collections.Generic;
using PlayGen.SUGAR.Core.Controllers;
using PlayGen.SUGAR.Core.Sessions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
	public class ProgressEvaluator
	{
		private readonly EvaluationController _evaluationController;

		public ProgressEvaluator(EvaluationController evaluationController)
		{
			_evaluationController = evaluationController;
		}

		public ConcurrentProgressCache EvaluateActor(List<Evaluation> evaluations, Session session)
		{
			var progress = new ConcurrentProgressCache();

			foreach (var evaluation in evaluations)
				AddProgress(progress, evaluation, session);

			return progress;
		}

		public ConcurrentProgressCache EvaluateSessions(ICollection<Session> sessions, Evaluation evaluation)
		{
			var progress = new ConcurrentProgressCache();

			foreach (var session in sessions)
				AddProgress(progress, evaluation, session);

			return progress;
		}

		public ConcurrentProgressCache EvaluateSessions(ICollection<Session> sessions, ICollection<Evaluation> evaluations)
		{
			var progress = new ConcurrentProgressCache();

			foreach (var session in sessions)
			foreach (var evaluation in evaluations)
				AddProgress(progress, evaluation, session);

			return progress;
		}

		private void AddProgress(ConcurrentProgressCache concurrentProgress, Evaluation evaluation, Session session)
		{
			if (!_evaluationController.IsAlreadyCompleted(evaluation, session.ActorId))
			{
				var progressValue = _evaluationController.EvaluateProgress(evaluation, session.ActorId);
				concurrentProgress.AddProgress(session.GameId, session.ActorId, evaluation, progressValue);
			}
		}
	}
}