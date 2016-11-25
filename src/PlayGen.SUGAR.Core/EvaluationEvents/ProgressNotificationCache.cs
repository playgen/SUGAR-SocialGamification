using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    /// <summary>
    /// Check newly evaluated progress and store notifications for values of any significance
    /// </summary>
    public class ProgressNotificationCache
    {
        // <gameId, <actorId, <evaluation, progress>>>
        // Latest stored last in <evaluation, progress> list.
        private readonly Dictionary<int, Dictionary<int, List<KeyValuePair<Evaluation, float>>>> _pendingNotifications = new Dictionary<int, Dictionary<int, List<KeyValuePair<Evaluation, float>>>>();
       
        public bool Remove(int? gameId, int actorId)
        {
            var didRemove = false;

            Dictionary<int, List<KeyValuePair<Evaluation, float>>> gameProgress;
            if (_pendingNotifications.TryGetValue(gameId.ToInt(), out gameProgress))
            {
                if (gameProgress.Remove(actorId))
                {
                    didRemove = true;

                    if (gameProgress.Count == 0)
                    {
                        _pendingNotifications.Remove(gameId.ToInt());
                    }
                }
            }

            return didRemove;
        }

        public bool Remove(int evaluationId)
        {
            var didRemove = false;
            var actorsToRemove = new List<KeyValuePair<int, int>>();    // <gameId, actorId>

            foreach (var gameProgress in _pendingNotifications)
            {
                foreach (var actorProgress in gameProgress.Value)
                {
                    if (actorProgress.Value.RemoveAll(p => p.Key.Id == evaluationId) > 0)
                    {
                        didRemove = true;

                        if (actorProgress.Value.Count == 0)
                        {
                            actorsToRemove.Add(new KeyValuePair<int, int>(gameProgress.Key, actorProgress.Key));
                        }
                    }
                }
            }

            PruneActors(actorsToRemove);
            
            return didRemove;
        }

        public void Update(ProgressCache addProgress)
        {
            foreach (var addGameProgress in addProgress)
            {
                Dictionary<int, List<KeyValuePair<Evaluation, float>>> existingGameProgress;
                if (!_pendingNotifications.TryGetValue(addGameProgress.Key, out existingGameProgress))
                {
                    existingGameProgress = new Dictionary<int, List<KeyValuePair<Evaluation, float>>>();
                    _pendingNotifications[addGameProgress.Key] = existingGameProgress;
                }

                foreach (var addActorProgress in addGameProgress.Value)
                {
                    List<KeyValuePair<Evaluation, float>> existingActorProgress;
                    if (!existingGameProgress.TryGetValue(addActorProgress.Key, out existingActorProgress))
                    {
                        existingActorProgress = new List<KeyValuePair<Evaluation, float>>();
                        existingGameProgress[addActorProgress.Key] = existingActorProgress;
                    }

                    foreach (var addEvaluationProgress in addActorProgress.Value)
                    {
                        existingActorProgress.RemoveAll(p => p.Key == addEvaluationProgress.Key);
                        existingActorProgress.Add(addEvaluationProgress);
                    }
                }
            }
        }

        public Dictionary<int, List<KeyValuePair<Evaluation, float>>> Get(int? gameId, int actorId)
        {
            var actorsProgress = new Dictionary<int, List<KeyValuePair<Evaluation, float>>>();

            List<KeyValuePair<Evaluation, float>> actorProgress;
            if (TryTake(gameId, actorId, out actorProgress))
            {
                actorsProgress[actorId] = actorProgress;
            }

            // todo also add progress updates for groups here
            return actorsProgress;
        }

        private bool TryTake(int? gameId, int actorId, out List<KeyValuePair<Evaluation, float>> actorProgress)
        {
            actorProgress = null;

            Dictionary<int, List<KeyValuePair<Evaluation, float>>> gameProgress;
            if (_pendingNotifications.TryGetValue(gameId.ToInt(), out gameProgress))
            {
                if (gameProgress.TryGetValue(actorId, out actorProgress))
                {
                    gameProgress.Remove(actorId);

                    if (gameProgress.Count == 0)
                    {
                        _pendingNotifications.Remove(gameId.ToInt());
                    }
                }
            }

            return actorProgress != null;
        }

        private void PruneActors(List<KeyValuePair<int, int>> actorsToRemove)
        {
            var gamesToRemove = new List<int>();

            foreach (var removeActor in actorsToRemove)
            {
                _pendingNotifications[removeActor.Key].Remove(removeActor.Value);

                if (_pendingNotifications[removeActor.Key].Count == 0)
                {
                    gamesToRemove.Add(removeActor.Key);
                }
            }

            foreach (var removeGame in gamesToRemove)
            {
                _pendingNotifications.Remove(removeGame);
            }
        }
    }
}
