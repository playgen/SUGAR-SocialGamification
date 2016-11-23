using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Core.UnitTests;
using PlayGen.SUGAR.Core.UnitTests.EvaluationEvents;
using PlayGen.SUGAR.Data.Model;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests.EvaluationEvents
{
    [Collection("Test Data Fixture Collection")] // todo change to use this one
    //[Collection("Project Fixture Collection")] // having 2 different kinds of project fixtures seems to cause issues and tests fail where they would normally pass.
    public class EvaluationGameDataMapperEvaluationTests : EvaluationTestsBase
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
                shouldGetEvaluations.Add(Helpers.ComposeGenericAchievement($"MapsExistingEvaluationsGameData_{i}"));
                shouldntGetEvaluations.Add(Helpers.ComposeGenericAchievement($"MapsExistingEvaluationsGameData_Ignore_{i}"));
            }

            // Act
            GameDataMapper.CreateMappings(shouldGetEvaluations);
            GameDataMapper.CreateMappings(shouldntGetEvaluations);

            // Assert
            foreach (var shouldGetEvaluation in shouldGetEvaluations)
            {
                foreach (var evaluationCriteria in shouldGetEvaluation.EvaluationCriterias)
                {
                    var gameData = Helpers.ComposeGameData(0, evaluationCriteria, shouldGetEvaluation.GameId);

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
            var removeEvaluation = Helpers.ComposeGenericAchievement($"RemovesEvaluationsGameDataMapping");

            var shouldntRemoveEvaluations = new List<Evaluation>(count);

            for (var i = 0; i < count; i++)
            {
                shouldntRemoveEvaluations.Add(Helpers.ComposeGenericAchievement($"RemovesEvaluationsGameDataMapping_ShouldntRemove_{i}"));
            }

            GameDataMapper.CreateMapping(removeEvaluation);
            GameDataMapper.CreateMappings(shouldntRemoveEvaluations);

            // Act
            GameDataMapper.RemoveMapping(removeEvaluation);

            // Assert
            // Make sure removed evaluation isn't returned
            foreach (var evaluationCriteria in removeEvaluation.EvaluationCriterias)
            {
                var gameData = Helpers.ComposeGameData(0, evaluationCriteria, removeEvaluation.GameId);

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
                    var gameData = Helpers.ComposeGameData(0, evaluationCriteria, shouldntRemoveEvaluation.GameId);

                    HashSet<Evaluation> relatedEvaluations;
                    var didGetRelated = GameDataMapper.TryGetRelated(gameData, out relatedEvaluations);

                    Assert.True(didGetRelated, "Shouldn't have removed unremoved evaluations.");
                    Assert.Contains(shouldntRemoveEvaluation, relatedEvaluations);
                }
            }
        }
    }
}