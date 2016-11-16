using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests.EvaluationEvents
{
    public class EvaluationGameDataMapperTests : TestsBase
    {
        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(20)]
        public void CreateAndGetRelated(int index)
        {
            // Arrange
            var gameId = TestData.Games[index].Id;
            var evaluation = CreateEvaluation("CreatesEvaluationGameDataMapping", gameId);
            var ignoreEvaluations = CreateEvaluation("CreatesEvaluationGameDataMapping_Ignore", gameId);

            // Act
            GameDataMapper.CreateMappings(evaluation);

            // Assert
            foreach (var evaluationCriteria in evaluation.EvaluationCriterias)
            {
                var gameData = new GameData
                {
                    Key = evaluationCriteria.Key,
                    DataType = evaluationCriteria.DataType,

                    GameId = gameId,
                    Value = "100"
                };

                HashSet<Evaluation> evaluations;
                var didGetRelated = GameDataMapper.TryGetRelated(gameData, out evaluations);

                Assert.True(didGetRelated, "Should have gotten related evaluations.");
                Assert.Contains(evaluation, evaluations);
                Assert.DoesNotContain(ignoreEvaluations, evaluations);
            }
        }

        public void RemovesEvaluationsGameDataMapping()
        {
            throw new NotImplementedException();
        }

        public void MapsExistingEvaluationsGameData()
        {
            throw new NotImplementedException();
        }
    }
}