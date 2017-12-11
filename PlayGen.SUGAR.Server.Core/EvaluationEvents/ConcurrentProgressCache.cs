using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.EvaluationEvents
{
	public class ConcurrentProgressCache
	{
		// <gameId, <actorId, <evaluation, progress>>>
		private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>>> _progress = new ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>>>();

		// out <actorId, <evaluation, progress>>
		public bool TryGetGameProgress(int gameId, out ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> gameProgress)
		{
			gameProgress = GetGameProgress(gameId);
			return gameProgress != null;
		}

		// out <evaluation, progress>
		public bool TryGetActorProgress(int gameId, int actorId, out ConcurrentDictionary<Evaluation, float> actorProgress)
		{
			actorProgress = GetActorProgress(gameId, actorId);
			return actorProgress != null;
		}

		public bool RemoveActor(int gameId, int actorId)
		{
			var didRemove = false;

			if (TryGetGameProgress(gameId, out var gameProgress))
			{
				didRemove = gameProgress.TryRemove(actorId,  out var _);

				if (gameProgress.Count == 0)
				{
					_progress.TryRemove(gameId, out var _);
				}
			}

			return didRemove;
		}

		public ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> TakeActorProgress(int gameId, int actorId)
		{
			var actorsProgress = new ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>>();

			if (TryGetGameProgress(gameId, out var gameProgress))
			{
				if (gameProgress.TryRemove(actorId, out var removedActorProgress))
				{
					actorsProgress[actorId] = removedActorProgress;
				}
			}

			return actorsProgress;
		}


		public void AddProgress(ConcurrentProgressCache addConcurrentProgressCache, Func<float, bool> addCondition = null)
		{
			foreach (var addGameProgress in addConcurrentProgressCache._progress)
			{
				if (!_progress.TryGetValue(addGameProgress.Key, out var existingGameProgress))
				{
					existingGameProgress = new ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>>();
					_progress[addGameProgress.Key] = existingGameProgress;
				}

				foreach (var addActorProgress in addGameProgress.Value)
				{
					if (!existingGameProgress.TryGetValue(addActorProgress.Key, out var existingActorProgress))
					{
						existingActorProgress = new ConcurrentDictionary<Evaluation, float>();
						existingGameProgress[addActorProgress.Key] = existingActorProgress;
					}

					foreach (var addEvaluationProgress in addActorProgress.Value)
					{
						if (addCondition == null || addCondition(addEvaluationProgress.Value))
						{
							existingActorProgress[addEvaluationProgress.Key] = addEvaluationProgress.Value;
						}
					}
				}
			}
		}

		public ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> GetGameProgress(int gameId)
		{
			_progress.TryGetValue(gameId, out var gameProgress);

			return gameProgress;
		}

		public bool Remove(int evaluationId)
		{
			var actorsToRemove = new List<KeyValuePair<int, int>>();    // <gameId, actorId>

			foreach (var gameProgress in _progress)
			{
				foreach (var actorProgress in gameProgress.Value)
				{
					var evaluationProgress = actorProgress.Value;

					foreach (var evaluation in evaluationProgress.Keys.Where(k => k.Id == evaluationId))
					{
						if (evaluationProgress.TryRemove(evaluation, out var _) && evaluationProgress.Count == 0)
						{
							actorsToRemove.Add(new KeyValuePair<int, int>(gameProgress.Key, actorProgress.Key));
						}
					}
				}
			}

			PruneActors(actorsToRemove);

			return false;
		}

		private void PruneActors(List<KeyValuePair<int, int>> actorsToRemove)
		{
			var gamesToRemove = new List<int>();

			foreach (var removeActor in actorsToRemove)
			{
				_progress[removeActor.Key].TryRemove(removeActor.Value, out var _);

				if (_progress[removeActor.Key].Count == 0)
				{
					gamesToRemove.Add(removeActor.Key);
				}
			}

			foreach (var removeGame in gamesToRemove)
			{
				_progress.TryRemove(removeGame, out var _);
			}
		}
	   
		// <evaluation, progress>
		public ConcurrentDictionary<Evaluation, float> GetActorProgress(int gameId, int actorId)
		{
			if (TryGetGameProgress(gameId, out var gameProgress))
			{
				gameProgress.TryGetValue(actorId, out var actorProgress);
				return actorProgress;
			}
			return null;
		}

		public void AddProgress(int gameId, int actorId, Evaluation evaluation, float progress)
		{
			ConcurrentDictionary<Evaluation, float> actorProgress = null;
			
			if (_progress.TryGetValue(gameId, out var gameProgress))
			{
				gameProgress.TryGetValue(actorId, out actorProgress);
			}
			else
			{
				gameProgress = new ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>>();
				_progress[gameId] = gameProgress;
			}

			if(actorProgress == null)
			{
				actorProgress = new ConcurrentDictionary<Evaluation, float>();
				gameProgress[actorId] = actorProgress;
			}
			else
			{
				// todo rather replace all occurences of Dictionary<evaluation, progress> with a custom dictionary that does evaluation key comparrison bu Evaluation.Id.
				var matchingEvaluation = actorProgress.Keys.SingleOrDefault(e => e.Id == evaluation.Id);
				if(matchingEvaluation != null)
				{
					actorProgress.TryRemove(evaluation, out var _);
				}
			}

			actorProgress[evaluation] = progress;
		}
	}
}
