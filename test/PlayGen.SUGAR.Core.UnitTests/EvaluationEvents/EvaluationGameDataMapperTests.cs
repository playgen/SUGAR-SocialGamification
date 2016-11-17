using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests.EvaluationEvents
{
    public class EvaluationGameDataMapperTests : TestsBase
    {
        [Fact]
        public void CreateAndGetRelated()
        {
            // Arrange
            var count = 10;
            var shouldGetEvaluations = new List<Evaluation>(count);
            var shouldntGetEvaluations = new List<Evaluation>(count);

            for (var i = 0; i < count; i++)
            {
                var gameId = TestData.Games[i].Id;

                shouldGetEvaluations.Add(CreateEvaluation($"MapsExistingEvaluationsGameData_{i}", gameId));
                shouldntGetEvaluations.Add(CreateEvaluation($"MapsExistingEvaluationsGameData_Ignore_{i}", gameId));
            }

            // Act
            GameDataMapper.CreateMappings(shouldGetEvaluations);
            GameDataMapper.CreateMappings(shouldntGetEvaluations);

            // Assert
            foreach (var shouldGetEvaluation in shouldGetEvaluations)
            {
                foreach (var evaluationCriteria in shouldGetEvaluation.EvaluationCriterias)
                {
                    var gameData = CreateGameData(evaluationCriteria, shouldGetEvaluation.GameId);

                    HashSet<Evaluation> relatedEvaluations;
                    var didGetRelated = GameDataMapper.TryGetRelated(gameData, out relatedEvaluations);

                    Assert.True(didGetRelated, "Should have gotten related evaluations.");
                    Assert.Contains(shouldGetEvaluation, relatedEvaluations);

                    shouldntGetEvaluations.ForEach(sge => Assert.DoesNotContain(sge, relatedEvaluations));
                }
            }
        }

        [Fact]
        public void RemovesEvaluationsGameDataMapping()
        {
            // Arrange
            var count = 10;
            var gameId = TestData.Games[0].Id;
            var removeEvaluation = CreateEvaluation($"RemovesEvaluationsGameDataMapping", gameId);

            var shouldntRemoveEvaluations = new List<Evaluation>(count);

            for (var i = 0; i < count; i++)
            {
                gameId = TestData.Games[i].Id;
                shouldntRemoveEvaluations.Add(CreateEvaluation($"RemovesEvaluationsGameDataMapping_ShouldntRemove_{i}", gameId));
            }

            GameDataMapper.CreateMapping(removeEvaluation);
            GameDataMapper.CreateMappings(shouldntRemoveEvaluations);

            // Act
            GameDataMapper.RemoveMapping(removeEvaluation);

            // Assert
            // Make sure removed evaluation isn't returned
            foreach (var evaluationCriteria in removeEvaluation.EvaluationCriterias)
            {
                var gameData = CreateGameData(evaluationCriteria, removeEvaluation.GameId);

                HashSet<Evaluation> relatedEvaluations;
                var didGetRelated = GameDataMapper.TryGetRelated(gameData, out relatedEvaluations);

                // Either shouldn't have gotten related or if did, shouldn't have returned the removed evaluation
                if (didGetRelated)
                {
                    Assert.DoesNotContain(removeEvaluation, relatedEvaluations);
                }
            }

            // Make sure other evaluations still exist
            foreach (var shouldntRemoveEvaluation in shouldntRemoveEvaluations)
            {
                foreach (var evaluationCriteria in shouldntRemoveEvaluation.EvaluationCriterias)
                {
                    var gameData = CreateGameData(evaluationCriteria, shouldntRemoveEvaluation.GameId);

                    HashSet<Evaluation> relatedEvaluations;
                    var didGetRelated = GameDataMapper.TryGetRelated(gameData, out relatedEvaluations);

                    Assert.True(didGetRelated, "Shouldn't have removed unremoved evaluations.");
                    Assert.Contains(shouldntRemoveEvaluation, relatedEvaluations);
                }
            }
        }
    }
}