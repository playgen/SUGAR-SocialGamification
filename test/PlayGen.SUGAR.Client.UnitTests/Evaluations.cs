using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client.UnitTests
{
    public abstract class Evaluations
    {
        protected readonly TestSUGARClient TestSugarClient = new TestSUGARClient();
        protected readonly GameDataClient GameDataClient;

        protected Evaluations()
        {
            GameDataClient = TestSugarClient.GameData;

			Helpers.CreateAndLogin(TestSugarClient.Session);
        }

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
                        DataType = SaveDataType.Long,
                        Key = key,
                        Value = $"{100}",
                    }
                },
                Name = key,
                Token = key,
            });
        }

        protected void CompleteGenericEvaluation(EvaluationResponse evaluation, int userId)
        {
            GameDataClient.Add(new SaveDataRequest
            {
                ActorId = userId,
                SaveDataType = evaluation.EvaluationCriterias[0].DataType,
                Value = $"{200}",
                Key = evaluation.EvaluationCriterias[0].Key
            });
        }
    }
}
