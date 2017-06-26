using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Extensions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
	public class ConcurrentProgressCache
	{
		// <gameId, <actorId, <evaluation, progress>>>
		private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>>>
			_progress = new ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>>>();

		// out <actorId, <evaluation, progress>>
		public bool TryGetGameProgress(int? gameId,
			out ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> gameProgress)
		{
			gameProgress = GetGameProgress(gameId);
			return gameProgress != null;
		}

		// out <evaluation, progress>
		public bool TryGetActorProgress(int? gameId, int actorId, out ConcurrentDictionary<Evaluation, float> actorProgress)
		{
			actorProgress = GetActorProgress(gameId, actorId);
			return actorProgress != null;
		}

		public bool RemoveActor(int? gameId, int actorId)
		{
			var didRemove = false;

			ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> gameProgress;
			if (TryGetGameProgress(gameId, out gameProgress))
			{
				ConcurrentDictionary<Evaluation, float> removedActorProgress;
				didRemove = gameProgress.TryRemove(actorId, out removedActorProgress);

				if (gameProgress.Count == 0)
				{
					ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> removedGameProgress;
					_progress.TryRemove(gameId.ToInt(), out removedGameProgress);
				}
			}

			return didRemove;
		}

		public ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> TakeActorProgress(int? gameId, int actorId)
		{
			var actorsProgress =
				new ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>>();

			ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> gameProgress;
			if (TryGetGameProgress(gameId, out gameProgress))
			{
				ConcurrentDictionary<Evaluation, float> removedActorProgress;
				if (gameProgress.TryRemove(actorId, out removedActorProgress))
					actorsProgress[actorId] = removedActorProgress;
			}

			return actorsProgress;
		}


		public void AddProgress(ConcurrentProgressCache addConcurrentProgressCache, Func<float, bool> addCondition = null)
		{
			foreach (var addGameProgress in addConcurrentProgressCache._progress)
			{
				ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> existingGameProgress;
				if (!_progress.TryGetValue(addGameProgress.Key, out existingGameProgress))
				{
					existingGameProgress = new ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>>();
					_progress[addGameProgress.Key] = existingGameProgress;
				}

				foreach (var addActorProgress in addGameProgress.Value)
				{
					ConcurrentDictionary<Evaluation, float> existingActorProgress;
					if (!existingGameProgress.TryGetValue(addActorProgress.Key, out existingActorProgress))
					{
						existingActorProgress = new ConcurrentDictionary<Evaluation, float>();
						existingGameProgress[addActorProgress.Key] = existingActorProgress;
					}

					foreach (var addEvaluationProgress in addActorProgress.Value)
						if (addCondition == null || addCondition(addEvaluationProgress.Value))
							existingActorProgress[addEvaluationProgress.Key] = addEvaluationProgress.Value;
				}
			}
		}

		public ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> GetGameProgress(int? gameId)
		{
			ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> gameProgress;
			_progress.TryGetValue(gameId.ToInt(), out gameProgress);

			return gameProgress;
		}

		public bool Remove(int evaluationId)
		{
			var didRemove = false;
			var actorsToRemove = new List<KeyValuePair<int, int>>(); // <gameId, actorId>

			foreach (var gameProgress in _progress)
			foreach (var actorProgress in gameProgress.Value)
			{
				var evaluationProgress = actorProgress.Value;

				foreach (var evaluation in evaluationProgress.Keys.Where(k => k.Id == evaluationId))
				{
					float removedEvaluationProgress;
					if (evaluationProgress.TryRemove(evaluation, out removedEvaluationProgress) && evaluationProgress.Count == 0)
						actorsToRemove.Add(new KeyValuePair<int, int>(gameProgress.Key, actorProgress.Key));
				}
			}

			PruneActors(actorsToRemove);

			return didRemove;
		}

		private void PruneActors(List<KeyValuePair<int, int>> actorsToRemove)
		{
			var gamesToRemove = new List<int>();

			foreach (var removeActor in actorsToRemove)
			{
				ConcurrentDictionary<Evaluation, float> removedEvaluationProgress;
				_progress[removeActor.Key]
					.TryRemove(removeActor.Value, out removedEvaluationProgress);

				if (_progress[removeActor.Key]
						.Count == 0)
					gamesToRemove.Add(removeActor.Key);
			}

			foreach (var removeGame in gamesToRemove)
			{
				ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> removedGameProgress;
				_progress.TryRemove(removeGame, out removedGameProgress);
			}
		}

		// <evaluation, progress>
		public ConcurrentDictionary<Evaluation, float> GetActorProgress(int? gameId, int actorId)
		{
			ConcurrentDictionary<Evaluation, float> actorProgress = null;

			ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> gameProgress;
			if (TryGetGameProgress(gameId, out gameProgress))
				gameProgress.TryGetValue(actorId, out actorProgress);

			return actorProgress;
		}

		public void AddProgress(int? gameId, int actorId, Evaluation evaluation, float progress)
		{
			ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> gameProgress;
			ConcurrentDictionary<Evaluation, float> actorProgress = null;

			if (_progress.TryGetValue(gameId.ToInt(), out gameProgress))
			{
				gameProgress.TryGetValue(actorId, out actorProgress);
			}
			else
			{
				gameProgress = new ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>>();
				_progress[gameId.ToInt()] = gameProgress;
			}

			if (actorProgress == null)
			{
				actorProgress = new ConcurrentDictionary<Evaluation, float>();
				gameProgress[actorId] = actorProgress;
			}
			else
			{
				// todo rather replace all occurences of Dictionary<evaluation, progress> with a custom dictionary that does evaluation key comparrison bu Evaluation.Id.
				var matchingEvaluation = actorProgress.Keys.SingleOrDefault(e => e.Id == evaluation.Id);
				if (matchingEvaluation != null)
				{
					float removedEvaluationProgress;
					actorProgress.TryRemove(evaluation, out removedEvaluationProgress);
				}
			}

			actorProgress[evaluation] = progress;
		}
	}
}