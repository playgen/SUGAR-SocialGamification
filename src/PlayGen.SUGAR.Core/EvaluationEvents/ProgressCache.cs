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
        // <gameId, <actorId, <evaluationId, progress>>>
        private readonly Dictionary<int, Dictionary<int, Dictionary<int, float>>> _progressMappings = new Dictionary<int, Dictionary<int, Dictionary<int, float>>>();
        private readonly CriteriaEvaluator _evaluationCriteriaEvaluator;

        public ProgressCache(CriteriaEvaluator evaluationCriteriaEvaluator)
        {
            _evaluationCriteriaEvaluator = evaluationCriteriaEvaluator;
        }

        internal Dictionary<int, Dictionary<int, Dictionary<int, float>>> StartTracking(int gameId, int actorId)
        {
            // todo add game and user to list to evaluate against
            EvaluateActor(gameId, actorId);
            throw new NotImplementedException();
        }

        internal void StopTracking(int gameId, int actorId)
        {
            // todo remove progress for user for game
            throw new NotImplementedException();
        }

        internal Dictionary<int, Dictionary<int, Dictionary<int, float>>> EvaluateActor(int gameId, int actorId)
        {
            // todo run all evaluations for this game against this user's data and store the results

            // todo if the evalution was completed, trigger an event to notify the user of the completion next time they 
            // request their achievements - keep an evaluation progress stack that users can query to get their status

            throw new NotImplementedException();
        }

        internal Dictionary<int, Dictionary<int, Dictionary<int, float>>> Evaluate(Evaluation evaluation)
        {
            var affectedActors = GetAffectedActors(evaluation);

            foreach (var actorId in affectedActors)
            {
                EvaluateActor(evaluation, actorId);
            }

            throw new NotImplementedException();
        }

        internal Dictionary<int, Dictionary<int, Dictionary<int, float>>> Evaluate(IEnumerable<Evaluation> evaluations, GameData gameData)
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

        private Dictionary<int, Dictionary<int, Dictionary<int, float>>> EvaluateActor(Evaluation evaluation, int actorId)
        {
            // todo evaluate against te actor
            throw new NotImplementedException();
        }

        internal void Remove(int evaluationId)
        {
            // todo remove all progress for this evaluation
            throw new NotImplementedException();
        }

        internal IReadOnlyDictionary<int, float> GetProgress(int gameId, int actorId)
        {
            var progress = _progressMappings[gameId][actorId];
            return new ReadOnlyDictionary<int, float>(progress);
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
