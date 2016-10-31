using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using NUnit.Framework;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class AchievementControllerTests
	{
		#region Configuration
		private readonly AchievementController _achievementDbController;
		private readonly GameController _gameController;

		public AchievementControllerTests()
		{
			_achievementDbController = TestEnvironment.AchievementController;
			_gameController = TestEnvironment.GameController;
		}
		#endregion

		
		#region Tests
		[Test]
		public void CreateAndGetAchievement()
		{
			string achievementName = "CreateAchievement";

			var newAchievement = CreateAchievement(achievementName);

			var achievement = _achievementDbController.Get(newAchievement.Token, newAchievement.GameId);

			Assert.AreEqual(achievementName, achievement.Name);
		}

		[Test]
		public void CreateAndGetGlobalAchievement()
		{
			string achievementName = "CreateGlobalAchievement";

			var newAchievement = CreateAchievement(achievementName, 0);

			var achievement = _achievementDbController.Get(newAchievement.Token, newAchievement.GameId);

			Assert.AreEqual(achievementName, achievement.Name);
		}

		[Test]
		public void CreateDuplicateAchievement()
		{
			string achievementName = "CreateDuplicateAchievement";

			var firstachievement = CreateAchievement(achievementName);

			Assert.Throws<DuplicateRecordException>(() => CreateAchievement(achievementName, firstachievement.GameId));
		}

		[Test]
		public void GetAchievementsByGame()
		{
			var baseAchievement = CreateAchievement("GetAchievementsByBaseGame");

			int gameId = baseAchievement.GameId;

			string[] names = new[]
			{
				"GetAchievementsByGame1",
				"GetAchievementsByGame2",
				"GetAchievementsByGame3",
				"GetAchievementsByGame4",
			};

			foreach (var name in names)
			{
				CreateAchievement(name, gameId);
			}

			var achievements = _achievementDbController.GetByGame(gameId);

			var matching = achievements.Where(a => names.Contains(a.Name));

			Assert.AreEqual(names.Length, matching.Count());
		}

		[Test]
		public void GetAchievementsByNonExistingGame()
		{
			var achievements = _achievementDbController.GetByGame(-1);

			Assert.IsEmpty(achievements);
		}

		[Test]
		public void GetNonExistingAchievement()
		{
			var achievement = _achievementDbController.Get("GetNonExistingAchievement", -1);

			Assert.Null(achievement);
		}

		[Test]
		public void UpdateAchievement()
		{
			string achievementName = "UpdateExistingAchievement";

			Achievement newAchievement = CreateAchievement(achievementName);

			var foundAchievement = _achievementDbController.Get(newAchievement.Token, newAchievement.GameId);

			Assert.NotNull(foundAchievement);

			var update = new Achievement
			{
				Name = newAchievement.Name + "Updated",
				Token = newAchievement.Token,
				GameId = newAchievement.GameId,
				ActorType = newAchievement.ActorType,
				CompletionCriteriaCollection = newAchievement.CompletionCriteriaCollection,
				RewardCollection = newAchievement.RewardCollection
			};

			_achievementDbController.Update(update);

			var updatedAchievement = _achievementDbController.Get(newAchievement.Token, newAchievement.GameId);

			Assert.AreNotEqual(foundAchievement.Name, updatedAchievement.Name);
			Assert.AreEqual(foundAchievement.Name + "Updated", updatedAchievement.Name);
		}

		[Test]
		public void UpdateAchievementToDuplicateName()
		{
			string achievementName = "UpdateAchievementToDuplicateName";

			Achievement newAchievement = CreateAchievement(achievementName);

			Achievement newAchievementDuplicate = CreateAchievement(achievementName + " Two", newAchievement.GameId);

			var update = new Achievement
			{
				Name = achievementName,
				Token = newAchievementDuplicate.Token,
				GameId = newAchievementDuplicate.GameId,
				ActorType = newAchievementDuplicate.ActorType,
				CompletionCriteriaCollection = newAchievementDuplicate.CompletionCriteriaCollection,
				RewardCollection = newAchievementDuplicate.RewardCollection
			};

			Assert.Throws<DuplicateRecordException>(() => _achievementDbController.Update(update));
		}

		[Test]
		public void UpdateNonExistingAchievement()
		{
			string achievementName = "UpdateNonExistingAchievement";

			var achievement = new Achievement
			{
				Name = achievementName,
				Token = achievementName,
				GameId = -1,
				ActorType = ActorType.User,
				CompletionCriteriaCollection = new AchievementCriteriaCollection(),
				RewardCollection = new RewardCollection()
			};

			Assert.Throws<MissingRecordException>(() => _achievementDbController.Update(achievement));
		}

		[Test]
		public void DeleteExistingAchievement()
		{
			string achievementName = "DeleteExistingAchievement";

			var achievement = CreateAchievement(achievementName);

			var achievementReturned = _achievementDbController.Get(achievement.Token, achievement.GameId);
			Assert.NotNull(achievementReturned);
			Assert.AreEqual(achievementReturned.Name, achievementName);

			_achievementDbController.Delete(achievement.Token, achievement.GameId);
			achievementReturned = _achievementDbController.Get(achievement.Token, achievement.GameId);

			Assert.Null(achievementReturned);
		}

		[Test]
		public void DeleteNonExistingGroupAchievement()
		{
			_achievementDbController.Delete("DeleteNonExistingGroupAchievement", -1);
		}
		#endregion

		#region Helpers
		private Achievement CreateAchievement(string name, int? gameId = null, bool addCriteria = true)
		{
			if (gameId == null) {
				Game game = new Game
				{
					Name = name
				};
				_gameController.Create(game);
				gameId = game.Id;
			}

			var achievement = new Achievement
			{
				Name = name,
				Token = name,
				GameId = gameId.Value,
				ActorType = ActorType.User,
				CompletionCriteriaCollection = new AchievementCriteriaCollection(),
				RewardCollection = new RewardCollection()
			};
			if (addCriteria)
			{
				var criteria = new AchievementCriteriaCollection
				{
					new AchievementCriteria
					{
						Key = "CreateAchievementKey",
						DataType = GameDataType.String,
						CriteriaQueryType = CriteriaQueryType.Any,
						ComparisonType = ComparisonType.Equals,
						Scope = CriteriaScope.Actor,
						Value = "CreateAchievementValue"
					}
				};
				achievement.CompletionCriteriaCollection = criteria;
			}

			_achievementDbController.Create(achievement);

			return achievement;
		}
		#endregion
	}
}
 