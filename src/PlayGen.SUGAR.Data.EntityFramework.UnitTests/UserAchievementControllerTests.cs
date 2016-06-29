using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class UserAchievementControllerTests : TestController
	{
		#region Configuration
		private readonly AchievementController _userAchievementDbController;

		public UserAchievementControllerTests()
		{
			_userAchievementDbController = new AchievementController(NameOrConnectionString);
		}
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetUserAchievement()
		{
			string userAchievementName = "CreateUserAchievement";

			var newAchievement = CreateUserAchievement(userAchievementName);

			var userAchievements = _userAchievementDbController.Get(new int[] { newAchievement.Id });

			int matches = userAchievements.Count(g => g.Name == userAchievementName && g.GameId == newAchievement.GameId);

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateUserAchievementWithNonExistingGame()
		{
			string userAchievementName = "CreateUserAchievementWithNonExistingGame";
			Assert.Throws<MissingRecordException>(() => CreateUserAchievement(userAchievementName, -1));
		}

		[Fact]
		public void CreateDuplicateUserAchievement()
		{
			string userAchievementName = "CreateDuplicateUserAchievement";

			var firstachievement = CreateUserAchievement(userAchievementName);

			Assert.Throws<DuplicateRecordException>(() => CreateUserAchievement(userAchievementName, firstachievement.GameId.Value));			
		}

		[Fact]
		public void GetNonExistingUserAchievements()
		{
			var userAchievements = _userAchievementDbController.Get(new int[] { -1 });

			Assert.Empty(userAchievements);
		}

		[Fact]
		public void DeleteExistingUserAchievement()
		{
			string userAchievementName = "DeleteExistingUserAchievement";

			var userAchievement = CreateUserAchievement(userAchievementName);
			var userId = userAchievement.Id;

			var userAchievements = _userAchievementDbController.Get(new int[] { userId });
			Assert.Equal(userAchievements.Count(), 1);
			Assert.Equal(userAchievements.ElementAt(0).Name, userAchievementName);

			_userAchievementDbController.Delete(userAchievement.Id);
			userAchievements = _userAchievementDbController.Get(new int[] { userId });

			Assert.Empty(userAchievements);
		}

		[Fact]
		public void DeleteNonExistingUserAchievement()
		{
			_userAchievementDbController.Delete(-1);
		}
		#endregion

		#region Helpers
		private Achievement CreateUserAchievement(string name, int gameId = 0)
		{
			GameController gameDbController = new GameController(NameOrConnectionString);
			if (gameId == 0)
			{
				Game game = new Game
				{
					Name = name
				};
				gameDbController.Create(game);
				gameId = game.Id;
			}

			var userAchievement = new Achievement
			{
				Name = name,
				GameId = gameId,
				CompletionCriteriaCollection = new AchievementCriteriaCollection(),
				RewardCollection = new RewardCollection()
			};
			_userAchievementDbController.Create(userAchievement);

			return userAchievement;
		}
		#endregion
	}
}