using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;
/*
namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GroupAchievementControllerTests : TestController
	{
		#region Configuration
		private readonly AchievementController _groupAchievementDbController;

		public GroupAchievementControllerTests()
		{
			_groupAchievementDbController = new AchievementController(NameOrConnectionString);
		}
		#endregion


		#region Tests
		[Fact]
		public void CreateAndGetGroupAchievement()
		{
			string groupAchievementName = "CreateGroupAchievement";

			var newAchievement = CreateGroupAchievement(groupAchievementName);

			var groupAchievements = _groupAchievementDbController.Get(new int[] { newAchievement.Id });

			int matches = groupAchievements.Count(g => g.Name == groupAchievementName && g.GameId == newAchievement.GameId);

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateGroupAchievementWithNonExistingGame()
		{
			string groupAchievementName = "CreateGroupAchievementWithNonExistingGame";
			Assert.Throws<MissingRecordException>(() => CreateGroupAchievement(groupAchievementName, -1));
		}

		[Fact]
		public void CreateDuplicateGroupAchievement()
		{
			string groupAchievementName = "CreateDuplicateGroupAchievement";

			var firstachievement = CreateGroupAchievement(groupAchievementName);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupAchievement(groupAchievementName, firstachievement.GameId.Value));
		}

		[Fact]
		public void GetNonExistingGroupAchievements()
		{
			var groupAchievements = _groupAchievementDbController.Get(new int[] { -1 });

			Assert.Empty(groupAchievements);
		}

		[Fact]
		public void DeleteExistingGroupAchievement()
		{
			string groupAchievementName = "DeleteExistingGroupAchievement";

			var groupAchievement = CreateGroupAchievement(groupAchievementName);
			var groupId = groupAchievement.Id;

			var groupAchievements = _groupAchievementDbController.Get(new int[] { groupId });
			Assert.Equal(groupAchievements.Count(), 1);
			Assert.Equal(groupAchievements.ElementAt(0).Name, groupAchievementName);

			_groupAchievementDbController.Delete(groupAchievement.Id);
			groupAchievements = _groupAchievementDbController.Get(new int[] { groupId });

			Assert.Empty(groupAchievements);
		}

		[Fact]
		public void DeleteNonExistingGroupAchievement()
		{
			_groupAchievementDbController.Delete(-1);
		}
		#endregion

		#region Helpers
		private Achievement CreateGroupAchievement(string name, int gameId = 0)
		{
			GameController gameDbController = new GameController(NameOrConnectionString);
			if (gameId == 0) {
				Game game = new Game
				{
					Name = name
				};
				gameDbController.Create(game);
				gameId = game.Id;
			}

			var groupAchievement = new Achievement
			{
				Name = name,
				GameId = gameId,
				CompletionCriteriaCollection = new AchievementCriteriaCollection(),
				RewardCollection = new RewardCollection()
			};
			_groupAchievementDbController.Create(groupAchievement);

			return groupAchievement;
		}
		#endregion
	}
}
*/