// todo v2.0: support group evaluations
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlayGen.SUGAR.Data.Model;
using System.Linq;
using PlayGen.SUGAR.Core.Controllers;
using PlayGen.SUGAR.Core.Sessions;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
	public class ProgressEvaluator
	{
		private readonly EvaluationController _evaluationController;

		public ProgressEvaluator(EvaluationController evaluationController)
		{
			_evaluationController = evaluationController;
		}

		public ProgressCache EvaluateActor(IEnumerable<Evaluation> evaluations, Session session)
		{
			var progress = new ProgressCache();

			foreach (var evaluation in evaluations)
			{
				AddProgress(progress, evaluation, session);
			}

			return progress;
		}

		public ProgressCache EvaluateSessions(IEnumerable<Session> sessions, Evaluation evaluation)
		{
			var progress = new ProgressCache();

			foreach (var session in sessions)
			{
				AddProgress(progress, evaluation, session);
			}

			return progress;
		}

		public ProgressCache EvaluateSessions(IEnumerable<Session> sessions, IEnumerable<Evaluation> evaluations)
		{
			var progress = new ProgressCache();

			foreach (var session in sessions)
			{
				foreach (var evaluation in evaluations)
				{
					AddProgress(progress, evaluation, session);
				}
			}

			return progress;
		}

		private void AddProgress(ProgressCache progress, Evaluation evaluation, Session session)
		{
		    if (!_evaluationController.IsAlreadyCompleted(evaluation, session.ActorId))
		    {
		        var progressValue = _evaluationController.EvaluateProgress(evaluation, session.ActorId);
		        progress.AddProgress(session.GameId, session.ActorId, evaluation, progressValue);
		    }
		}
	}
}
