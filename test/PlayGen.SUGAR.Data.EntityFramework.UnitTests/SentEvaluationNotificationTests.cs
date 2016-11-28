using PlayGen.SUGAR.Data.Model;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
    public class SentEvaluationNotificationTests
    {
        [Fact]
        public void CanCreateAndGet()
        {
            // Arrange
            var key = "CanCreateAndGet";
            var user = Helpers.CreateUser(key);
            var game = Helpers.CreateGame(key);
            var evaluation = Helpers.CreateAchievement(key, game.Id);
            
            var created = new SentEvaluationNotification
            {
                GameId = game.Id,
                ActorId = user.Id,
                EvaluationId = evaluation.Id,
                Progress = 1,
            };

            // Act
            ControllerLocator.SentEvaluationNotificationController.Create(created);
            var gotten = ControllerLocator.SentEvaluationNotificationController.Get(game.Id, user.Id, evaluation.Id);

            // Assert
            Assert.Equal(game.Id, gotten.GameId);
            Assert.Equal(user.Id, gotten.ActorId);
            Assert.Equal(evaluation.Id, gotten.EvaluationId);
        }

        [Fact]
        public void CanDelete()
        {
            // Arrange
            var key = "CanDelete";
            var user = Helpers.CreateUser(key);
            var game = Helpers.CreateGame(key);
            var evaluation = Helpers.CreateAchievement(key, game.Id);

            var created = new SentEvaluationNotification
            {
                GameId = game.Id,
                ActorId = user.Id,
                EvaluationId = evaluation.Id,
                Progress = 1,
            };

            ControllerLocator.SentEvaluationNotificationController.Create(created);

            // Act
            ControllerLocator.SentEvaluationNotificationController.Delete(created.GameId, created.ActorId, created.EvaluationId);

            // Assert
            ControllerLocator.SentEvaluationNotificationController.Get(game.Id, user.Id, evaluation.Id);
        }

        [Fact]
        public void CanUpdate()
        {
            // Arrange
            var key = "CanUpdate";
            var user = Helpers.CreateUser(key);
            var game = Helpers.CreateGame(key);
            var evaluation = Helpers.CreateAchievement(key, game.Id);
            var originalProgress = 0.5f;
            var updatedProgress = 1f;

            var notification = new SentEvaluationNotification
            {
                GameId = game.Id,
                ActorId = user.Id,
                EvaluationId = evaluation.Id,
                Progress = originalProgress,
            };

            notification = ControllerLocator.SentEvaluationNotificationController.Create(notification);

            // Act
            notification.Progress = updatedProgress;
            ControllerLocator.SentEvaluationNotificationController.Update(notification);

            // Assert
            notification = ControllerLocator.SentEvaluationNotificationController.Get(game.Id, user.Id, evaluation.Id);

            Assert.Equal(game.Id, notification.GameId);
            Assert.Equal(user.Id, notification.ActorId);
            Assert.Equal(evaluation.Id, notification.EvaluationId);
            Assert.Equal(updatedProgress, notification.Progress);
        }
    }
}
