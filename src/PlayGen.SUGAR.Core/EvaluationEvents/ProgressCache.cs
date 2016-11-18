// todo v2.0: support group evaluations
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlayGen.SUGAR.Data.Model;
using System.Linq;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    /// <summary>
    /// Evaluation progress on actor per game basis for every actor that has an active session.
    /// </summary>
    public class ProgressCache
    {
        // <gameId, <actorId, <evaluation, progress>>>
        private readonly Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> _progressMappings = new Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>>();
        private readonly CriteriaEvaluator _evaluationCriteriaEvaluator;

        public ProgressCache(CriteriaEvaluator evaluationCriteriaEvaluator)
        {
            _evaluationCriteriaEvaluator = evaluationCriteriaEvaluator;
        }

        public Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> StartTracking(int gameId, int actorId)
        {
            // todo add game and user to list to evaluate against
            EvaluateActor(gameId, actorId);
            throw new NotImplementedException();
        }

        public void StopTracking(int gameId, int actorId)
        {
            // todo remove progress for user for game
            throw new NotImplementedException();
        }

        // progress: <evaluationId, progress>
        public bool TryGetProgress(int gameId, int actorId, out Dictionary<Evaluation, float> progress, Evaluation evaluation = null)
        {
            var didGetProgress = false;
            progress = null;

            Dictionary<int, Dictionary<Evaluation, float>> gameProgress;

            if (_progressMappings.TryGetValue(gameId, out gameProgress))
            {
                Dictionary<Evaluation, float> actorProgress;

                if (gameProgress.TryGetValue(actorId, out actorProgress))
                {
                    if(evaluation != null)
                    {
                        float progressValue;

                        if (progress.TryGetValue(evaluation, out progressValue))
                        {
                            progress = new Dictionary<Evaluation, float>()
                            {
                                {evaluation, progressValue}
                            };
                        }
                        else
                        {
                            progress = null;
                        }
                    }

                    didGetProgress = progress.Any();
                }
            }

            return didGetProgress;
        }

        public Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> EvaluateActor(int gameId, int actorId)
        {
            // todo run all evaluations for this game against this user's data and store the results

            // todo if the evalution was completed, trigger an event to notify the user of the completion next time they 
            // request their achievements - keep an evaluation progress stack that users can query to get their status

            throw new NotImplementedException();
        }

        public Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> Evaluate(Evaluation evaluation)
        {
            var affectedActors = GetAffectedActors(evaluation);

            foreach (var actorId in affectedActors)
            {
                EvaluateActor(evaluation, actorId);
            }

            throw new NotImplementedException();
        }

        public Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> Evaluate(IEnumerable<Evaluation> evaluations, GameData gameData)
        {
            var affectedActorsByEvaluation = GetAffectedActorsForEvaluations(evaluations, gameData);

            foreach (var evaluation in affectedActorsByEvaluation.Keys)
            {
                foreach (var actorId in affectedActorsByEvaluation[evaluation])
                {
                    EvaluateActor(evaluation, actorId);
                }
            }

            throw new NotImplementedException();
        }

        private Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> EvaluateActor(Evaluation evaluation, int actorId)
        {
            // todo evaluate against te actor
            throw new NotImplementedException();
        }

        public void Remove(int evaluationId)
        {
            // todo remove all progress for this evaluation
            throw new NotImplementedException();
        }

        private Dictionary<Evaluation, List<int>> GetAffectedActorsForEvaluations(IEnumerable<Evaluation> evaluations, GameData gameData)
        {
            var affectedActorsByEvaluation = new Dictionary<Evaluation, List<int>>();

            foreach (var evaluation in evaluations)
            {
                affectedActorsByEvaluation[evaluation] = GetAffectedActors(evaluation, gameData);
            }

            return affectedActorsByEvaluation;
        }

        private List<int> GetAffectedActors(Evaluation evaluation)
        {
            // todo based on evaluation actor type and scope
            return _progressMappings[evaluation.GameId].Keys.ToList();
        }

        private List<int> GetAffectedActors(Evaluation evaluation, GameData gameData)
        {
            // todo based on evaluation actor type, scope and game data id determine which actors are affected for each evaluation
            // todo make sure actor id is in the tracked actor id list
            return new List<int>() { gameData.ActorId.Value }; // 
        }
    }
}
