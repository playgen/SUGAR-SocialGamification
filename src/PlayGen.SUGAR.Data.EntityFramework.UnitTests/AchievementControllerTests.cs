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

		public AchievementControllerTests(TestEnvironment testEnvironment)
		{
			_achievementController = testEnvironment.AchievementController;
		}
		#endregion

		#region Tests
		[Fact]
		public void CanCreateAndGet()
		{
			var newAchievement = new Achievement
			{
				Token = "CanCreateAndGet",
			};

			_achievementController.Create(newAchievement);

			_achievementController.Get();
		}
		#endregion
	}
}