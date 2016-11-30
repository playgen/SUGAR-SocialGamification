using System;
using System.Collections.Concurrent;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Common.Shared.Extensions;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    /// <summary>
    /// Mappings of game data keys to evaluations with criteria that make use of the specific keys.
    /// </summary>
    public class EvaluationGameDataMapper
    {
        // <gamedata key, <evaluationId, evaluation>>
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<int, Evaluation>> _mappings = new ConcurrentDictionary<string, ConcurrentDictionary<int, Evaluation>>();

        public bool TryGetRelated(GameData gameData, out IEnumerable<Evaluation> evaluations)
        {
            var didGetRelated = false;
            evaluations = null;
            var mappedKey = CreateMappingKey(gameData.GameId, gameData.SaveDataType, gameData.Key);

            ConcurrentDictionary<int, Evaluation> relatedEvalautions;
            if (_mappings.TryGetValue(mappedKey, out relatedEvalautions))
            {
                evaluations = relatedEvalautions.Values;
                didGetRelated = true;
            }

            return didGetRelated;
        }

        public void CreateMappings(IEnumerable<Evaluation> evaluations)
        {
            foreach (var evaluation in evaluations)
            {
                CreateMapping(evaluation);
            }   
        }

        public void CreateMapping(Evaluation evaluation)
        {
            foreach (var evaluationCriteria in evaluation.EvaluationCriterias)
            {
                var mappingKey = CreateMappingKey(evaluation.GameId, evaluationCriteria.DataType, evaluationCriteria.Key);

                ConcurrentDictionary<int, Evaluation> mappedEvaluationsForKey;

                if (!_mappings.TryGetValue(mappingKey, out mappedEvaluationsForKey))
                {
                    mappedEvaluationsForKey = new ConcurrentDictionary<int, Evaluation>();
                    _mappings[mappingKey] = mappedEvaluationsForKey;
                }

                mappedEvaluationsForKey[evaluation.Id] = evaluation;
            }
        }

        public void RemoveMapping(Evaluation evaluation)
        {
            foreach (var evaluationCriteria in evaluation.EvaluationCriterias)
            {
                var mappingKey = CreateMappingKey(evaluation.GameId, evaluationCriteria.DataType, evaluationCriteria.Key);

                ConcurrentDictionary<int, Evaluation> mappedEvaluationsForKey;
                if (_mappings.TryGetValue(mappingKey, out mappedEvaluationsForKey))
                {
                    Evaluation removedEvaluation;
                    if (mappedEvaluationsForKey.TryRemove(evaluation.Id, out removedEvaluation) && mappedEvaluationsForKey.Count == 0)
                    {
                        ConcurrentDictionary<int, Evaluation> removedEvaluations;
                        _mappings.TryRemove(mappingKey, out removedEvaluations);
                    }
                }
            }
        }

        private string CreateMappingKey(int? gameId, SaveDataType dataType, string gameDataKey)
        {
            return $"{gameId.ToInt()};{dataType};{gameDataKey}";
        }
    }
}
