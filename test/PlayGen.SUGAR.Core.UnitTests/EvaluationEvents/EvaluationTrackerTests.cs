using System;
using System.Linq;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests.EvaluationEvents
{
    [Collection("Project Fixture Collection")]
    public class EvaluationTrackerTests : EvaluationTestsBase
    {
        /// <summary>
        /// Make sure:
        /// - existing evaluations are mapped
        /// - progress for user session is returned
        /// </summary>
        [Fact]
        public void EvaluatesOnSessionStarted()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("EvaluatesOnSessionStarted");
            var user = Helpers.GetOrCreateUser("EvaluatesOnSessionStarted");

            var evaluation = Helpers.CreateAndCompleteGenericAchievement("EvaluatesOnSessionStarted", user.Id, game.Id);

            // Act
            SessionTracker.StartSession(game.Id, user.Id);

            // Assert
            var progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);

            Assert.True(progress.ContainsKey(user.Id)); // should have evalauted for this user
            Assert.True(progress[user.Id].Any(kvp => kvp.Key.Id == evaluation.Id));// Should have returned the progress for the evaluation
            Assert.Equal(1, progress[user.Id].Single(kvp => kvp.Key.Id == evaluation.Id).Value);//Completed evaluation should have progress value of 1
        }

        /// <summary>
        /// Progress removed when session ended
        /// </summary>
        [Fact]
        public void RemovesOnSessionEnded()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("RemovesOnSessionEnded");
            var user = Helpers.GetOrCreateUser("RemovesOnSessionEnded");

            Helpers.CreateAndCompleteGenericAchievement("RemovesOnSessionEnded", user.Id, game.Id);

            var session = SessionTracker.StartSession(game.Id, user.Id);

            var progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);
            Assert.True(progress.ContainsKey(user.Id)); // should have evalauted for this user

            // Act
            SessionTracker.EndSession(session.Id);

            // Assert
            progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);
            Assert.False(progress.ContainsKey(user.Id)); // Shouldn't have any progress for this user
        }

        /// <summary>
        /// No progress stored when there is no session for the user
        /// </summary>
        [Fact]
        public void NoEvaluationWhenNoSession()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("NoEvaluationWhenNoSession");
            var user = Helpers.GetOrCreateUser("NoEvaluationWhenNoSession");

            Helpers.CreateAndCompleteGenericAchievement("NoEvaluationWhenNoSession", user.Id, game.Id);

            // Act
            var progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);

            // Assert
            Assert.Equal(0, progress.Count);
        }

        /// <summary>
        /// Adding game data triggers evaluations
        /// </summary>
        [Fact]
        public void EvaluatesOnGameDataAdded()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("EvaluatesOnGameDataAdded");
            var user = Helpers.GetOrCreateUser("EvaluatesOnGameDataAdded");

            SessionTracker.StartSession(game.Id, user.Id);

            var evaluation = Helpers.CreateGenericAchievement("EvaluatesOnGameDataAdded", game.Id);

            var gameDatas = Helpers.ComposeAchievementGameDatas(user.Id, evaluation, "100");

            // Act
            gameDatas.ForEach(g => ControllerLocator.GameDataController.Add(g));

            // Assert
            var progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);

            Assert.True(progress.ContainsKey(user.Id)); // should have evalauted for this user
            Assert.True(progress[user.Id].Any(kvp => kvp.Key.Id == evaluation.Id));// Should have returned the progress for the evaluation
        }

        /// <summary>
        /// Adding multiple game data triggers evaluations
        /// </summary>
        [Fact]
        public void EvaluatesOnGameDatasAdded()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("EvaluatesOnGameDatasAdded");
            var user = Helpers.GetOrCreateUser("EvaluatesOnGameDatasAdded");

            SessionTracker.StartSession(game.Id, user.Id);

            var evaluation = Helpers.CreateGenericAchievement("EvaluatesOnGameDatasAdded", game.Id);

            // Act
            Helpers.CompleteGenericAchievement(evaluation, user.Id);

            // Assert
            var progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);

            Assert.True(progress.ContainsKey(user.Id)); // should have evalauted for this user
            Assert.True(progress[user.Id].Any(kvp => kvp.Key.Id == evaluation.Id));// Should have returned the progress for the evaluation
            Assert.Equal(1, progress[user.Id].Single(kvp => kvp.Key.Id == evaluation.Id).Value);//Completed evaluation should have progress value of 1
        }

        /// <summary>
        /// - add achievement
        /// - add game data (that doesn't satisfy the completion condition)
        /// - make sure progress isn't complete
        /// - modify achievement to make sure it is completed with the current amount of data
        /// - make sure progress is complete
        /// </summary>
        [Fact]
        public void EvaluatesOnEvaluationUpdated()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("EvaluatesOnEvaluationUpdated");
            var user = Helpers.GetOrCreateUser("EvaluatesOnEvaluationUpdated");

            SessionTracker.StartSession(game.Id, user.Id);

            var evaluation = Helpers.CreateGenericAchievement("EvaluatesOnEvaluationUpdated", game.Id);
            Helpers.CreateGenericAchievementGameData(evaluation, user.Id);

            var progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);
            if (progress?.ContainsKey(user.Id) == true)
            {
                Assert.False(progress[user.Id].Any(kvp => kvp.Key.Id == evaluation.Id));    // Make sure the evaluation wasn't returned when it wasn't completed
            }

            // Act
            foreach (var criteria in evaluation.EvaluationCriterias)
            {
                criteria.Value = $"{50}";
            }

            ControllerLocator.EvaluationController.Update(evaluation);

            // Assert
            progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);
            progress[user.Id].ForEach(kvp => Assert.Equal(1, kvp.Value)); // Progress should be completed by this point
        }

        /// <summary>
        /// No progress stored when there is no session for the user
        /// </summary>
        [Fact]
        public void NoEvaluationWhenNotComplete()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("NoEvaluationWhenNotComplete");
            var user = Helpers.GetOrCreateUser("NoEvaluationWhenNotComplete");

            var evaluation = Helpers.CreateGenericAchievement("NoEvaluationWhenNotComplete", game.Id);
            Helpers.CreateGenericAchievementGameData(evaluation, user.Id);

            // Act
            var progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);

            // Assert
            if (progress.ContainsKey(user.Id))
            {
                Assert.False(progress[user.Id].Any(p => p.Key.Id == evaluation.Id));
            }
        }

        /// <summary>
        /// - add achievement and complete achievement
        /// - remove achievement
        /// - make sure no progress is recorded for achievement
        /// </summary>
        [Fact]
        public void RemovesProgressOnEvaluationDeleted()
        {
            // Arange 
            var game = Helpers.GetOrCreateGame("RemovesProgressOnEvaluationDeleted");
            var user = Helpers.GetOrCreateUser("RemovesProgressOnEvaluationDeleted");

            SessionTracker.StartSession(game.Id, user.Id);

            var evaluation = Helpers.CreateAndCompleteGenericAchievement("RemovesProgressOnEvaluationDeleted", user.Id, game.Id);

            // Act
            ControllerLocator.EvaluationController.Delete(evaluation.Token, game.Id);

            // Assert
            var progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);

            // Make sure that there are no instances of the evaluation in the progress
            foreach(var actorProgress in progress)
            {
                Assert.False(actorProgress.Value.Any(kvp => kvp.Key.Id == evaluation.Id));
            }

        }

        /// <summary>
        /// Should remove this actors progress after it's been accessed
        /// </summary>
        [Fact]
        public void RemovesProgressWhenGotten()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("RemovesProgressWhenGotten");
            var user = Helpers.GetOrCreateUser("RemovesProgressWhenGotten");

            SessionTracker.StartSession(game.Id, user.Id);

            var evaluation = Helpers.CreateAndCompleteGenericAchievement("RemovesProgressWhenGotten", user.Id, game.Id);

            // Act
            var progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);

            // Assert
            progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);

            if (progress.ContainsKey(user.Id))
            {
                Assert.False(progress[user.Id].Any(p => p.Key.Id == evaluation.Id));
            }
        }

        /// <summary>
        /// Adding multiple game data triggers evaluations
        /// </summary>
        [Fact]
        public void DoesntGetAlreadyRecievedNotifications()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("DoesntGetAlreadyRecievedNotifications");
            var user = Helpers.GetOrCreateUser("DoesntGetAlreadyRecievedNotifications");

            SessionTracker.StartSession(game.Id, user.Id);

            var evaluation = Helpers.CreateAndCompleteGenericAchievement("DoesntGetAlreadyRecievedNotifications", user.Id, game.Id);
            var progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);

            Helpers.CompleteGenericAchievement(evaluation, game.Id);
            // Act
            progress = EvaluationTracker.GetPendingNotifications(game.Id, user.Id);

            // Assert
            if (progress.ContainsKey(user.Id))
            {
                Assert.False(progress[user.Id].Any(p => p.Key.Id == evaluation.Id));
            }
        }
    }
}
