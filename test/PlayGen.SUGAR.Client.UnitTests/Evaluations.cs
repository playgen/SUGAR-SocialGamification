using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client.UnitTests
{
    public abstract class Evaluations : ClientTestsBase
    {
        protected abstract EvaluationResponse CreateEvaluation(EvaluationCreateRequest achievementRequest);

        protected EvaluationResponse CreateGenericEvaluation(string key)
        {
            return CreateEvaluation(new EvaluationCreateRequest
            {
                ActorType = ActorType.User,
                Description = key,
                EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
                {
                    new EvaluationCriteriaCreateRequest
                    {
                        ComparisonType = ComparisonType.GreaterOrEqual,
                        CriteriaQueryType = CriteriaQueryType.Sum,
                        SaveDataType = SaveDataType.Long,
                        SaveDataKey = key,
                        Value = $"{100}",
                    }
                },
                Name = key,
                Token = key,
            });
        }

        protected void CompleteGenericEvaluation(EvaluationResponse evaluation, int userId)
        {
            SUGARClient.GameData.Add(new SaveDataRequest
            {
                ActorId = userId,
                SaveDataType = evaluation.EvaluationCriterias[0].SaveDataType,
                Value = $"{200}",
                Key = evaluation.EvaluationCriterias[0].SaveDataKey
            });
        }
    }
}
