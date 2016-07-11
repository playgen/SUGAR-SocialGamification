using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class AchievementControllerTests : IClassFixture<TestEnvironment>
	{
		#region Configuration
		private readonly AchievementController _achievementController;
        private readonly GameController _gameController;

		public AchievementControllerTests(TestEnvironment testEnvironment)
		{
			_achievementController = testEnvironment.AchievementController;
            _gameController = testEnvironment.GameController;

        }
		#endregion

		#region Tests
		[Fact]
		public void CanCreateAndGet()
		{
            var game = GetOrCreateGame("CanCreateAndGet");

            var newAchievement = new Achievement
            {
                Token = "CanCreateAndGet",
                GameId = game.Id,
                CompletionCriteriaCollection = new AchievementCriteriaCollection(),
                RewardCollection = new RewardCollection(),
			};

			_achievementController.Create(newAchievement);

			var foundAchievement = _achievementController.Get(newAchievement.Token, newAchievement.GameId);

			Assert.Equal(newAchievement.Token, foundAchievement.Token);
			Assert.Equal(newAchievement.Description, foundAchievement.Description);
			Assert.Equal(newAchievement.Name, foundAchievement.Name);
			Assert.Equal(newAchievement.GameId, foundAchievement.GameId);
			Assert.Equal(newAchievement.ActorType, foundAchievement.ActorType);		
		}
        #endregion

        #region Helpers
        private Game GetOrCreateGame(string name)
        {
            var game = FindGame(name);

            if (game == null)
            {
                _gameController.Create(new Game
                {
                    Name = name,
                });

                game = FindGame(name);
            }

            return game;
        }

        private Game FindGame(string name)
        {
            var games = _gameController.Search(name);

            return games.Any() 
                ? games.ElementAt(0) 
                : null;
        }
        #endregion
    }
}