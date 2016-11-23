using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;
using System.Linq;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    public class ProgressCache
    {
        // <gameId, <actorId, <evaluation, progress>>>
        private readonly Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> _progress = new Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>>();

        // out <actorId, <evaluation, progress>>
        public bool TryGetGameProgress(int? gameId, out Dictionary<int, Dictionary<Evaluation, float>> gameProgress)
        {
            gameProgress = GetGameProgress(gameId);
            return gameProgress != null;
        }

        // out <evaluation, progress>
        public bool TryGetActorProgress(int? gameId, int actorId, out Dictionary<Evaluation, float> actorProgress)
        {
            actorProgress = GetActorProgress(gameId, actorId);
            return actorProgress != null;
        }

        public bool TryGetEvaluationProgress(int? gameId, int actorId, Evaluation evaluation, out float progress)
        {
            progress = 0;
            var didGetEvaluationProgress = false;

            Dictionary<Evaluation, float> actorProgress;
            if (TryGetActorProgress(gameId, actorId, out actorProgress))
            {
                didGetEvaluationProgress = actorProgress.TryGetValue(evaluation, out progress);
            }

            return didGetEvaluationProgress;
        }

        public Dictionary<int, Dictionary<Evaluation, float>> GetGameProgress(int? gameId)
        {
            Dictionary<int, Dictionary<Evaluation, float>> gameProgress;
            _progress.TryGetValue(gameId.ToInt(), out gameProgress);

            return gameProgress;
        }

        // <evaluation, progress>
        public Dictionary<Evaluation, float> GetActorProgress(int? gameId, int actorId)
        {
            Dictionary<Evaluation, float> actorProgress = null;

            Dictionary<int, Dictionary<Evaluation, float>> gameProgress;
            if (TryGetGameProgress(gameId, out gameProgress))
            {
                gameProgress.TryGetValue(actorId, out actorProgress);
            }

            return actorProgress;
        }

        public void AddProgress(int? gameId, int actorId, Evaluation evaluation, float progress)
        {
            Dictionary<int, Dictionary<Evaluation, float>> gameProgress;
            Dictionary<Evaluation, float> actorProgress = null;
            
            if (_progress.TryGetValue(gameId.ToInt(), out gameProgress))
            {
                gameProgress.TryGetValue(actorId, out actorProgress);
            }
            else
            {
                gameProgress = new Dictionary<int, Dictionary<Evaluation, float>>();
                _progress[gameId.ToInt()] = gameProgress;
            }

            if(actorProgress == null)
            {
                actorProgress = new Dictionary<Evaluation, float>();
                gameProgress[actorId] = actorProgress;
            }

            actorProgress[evaluation] = progress;
        }

        public List<int> GetActors(int? gameId)
        {
            List<int> actorIds = new List<int>();

            Dictionary<int, Dictionary<Evaluation, float>> gameProgress;

            if (TryGetGameProgress(gameId, out gameProgress))
            {
                actorIds = gameProgress.Keys.ToList();
            }

            return actorIds;
        }
    }
}
