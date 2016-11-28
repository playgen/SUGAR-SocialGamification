using System;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using System.Linq;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    /// <summary>
    /// Mappings of game data keys to evaluations with criteria that make use of the specific keys.
    /// </summary>
    public class EvaluationGameDataMapper
    {
        // <gamedata key, <evaluationId, evaluation>>
        private readonly Dictionary<string, Dictionary<int, Evaluation>> _mappings = new Dictionary<string, Dictionary<int, Evaluation>>();

        public bool TryGetRelated(GameData gameData, out IEnumerable<Evaluation> evaluations)
        {
            var didGetRelated = false;
            evaluations = null;
            var mappedKey = CreateMappingKey(gameData.GameId, gameData.SaveDataType, gameData.Key);

            Dictionary<int, Evaluation> relatedEvalautions;
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

                Dictionary<int, Evaluation> mappedEvaluationsForKey;

                if (!_mappings.TryGetValue(mappingKey, out mappedEvaluationsForKey))
                {
                    mappedEvaluationsForKey = new Dictionary<int, Evaluation>();
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

                Dictionary<int, Evaluation> mappedEvaluationsForKey;

                if (_mappings.TryGetValue(mappingKey, out mappedEvaluationsForKey))
                {
                    if (mappedEvaluationsForKey.Remove(evaluation.Id) && mappedEvaluationsForKey.Count == 0)
                    {
                        _mappings.Remove(mappingKey);
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
