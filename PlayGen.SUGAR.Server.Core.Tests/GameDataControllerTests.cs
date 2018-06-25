using System;
using System.Diagnostics;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.Core.Tests
{
    public class GameDataControllerTests : CoreTestFixtureBase
    {
	    public GameDataControllerTests(CoreTestFixture fixture) : base(fixture)
	    {
	    }

        [Theory]
		[InlineData("a")]
		[InlineData("My_key0")]
		[InlineData("1mykey")]
		[InlineData("_key")]
		[InlineData("_")]
		[InlineData("9291")]
		public void CanCreateWithValidKey(string key)
        {
			// Arrange
			var controller = ControllerLocator.GameDataController;

			// Act
			var data = controller.Add(new EvaluationData
			{
				Category = EvaluationDataCategory.GameData,

				Key = key,

				Value = "TestValue",

				EvaluationDataType = EvaluationDataType.String
			});

            // Assert
			Assert.NotNull(data);
		}

		[Theory]
		[InlineData("")]
		[InlineData("$")]
		[InlineData("dj_+das")]
		public void CantCreateWithInValidKey(string key)
		{
			// Arrange
			var controller = ControllerLocator.GameDataController;

			// Act Assert
			Assert.Throws<ArgumentException>(() => controller.Add(new EvaluationData
			{
				Category = EvaluationDataCategory.GameData,

				Key = key,

				Value = "TestValue",

				EvaluationDataType = EvaluationDataType.String
			}));		
		}

	    [Theory]
	    [InlineData(1)]
	    [InlineData(12)]
        [InlineData(123)]
        [InlineData(1234)]
	    [InlineData(12345)]
        [InlineData(123456)]
        public void CanCreateMultipleWithinTime(int dataCount)
	    {
		    // Arrange
		    const int overheadMaxAllowance = 100;
			const int expectedMaxMillisecondsPerThousand = 2000;
		    var expectedMillisecondsTotal = ((dataCount / 1000f) * expectedMaxMillisecondsPerThousand) + overheadMaxAllowance;
			var key = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}_{dataCount}";
		    var gameId = Fixture.SortedGames[0].Id;
		    var actorId = Fixture.SortedUsers[0].Id;
            var data = Enumerable.Range(0, dataCount).Select(i => new EvaluationData
		    {
			    Category = EvaluationDataCategory.GameData,
				GameId = gameId,
				ActorId = actorId,
				Key = key,
				Value = i.ToString(),
				EvaluationDataType = EvaluationDataType.Long
			}).ToList();

            // Act 
		    var stopwatch = Stopwatch.StartNew();
		    ControllerLocator.GameDataController.Add(data);
			stopwatch.Stop();

			// Assert
			Assert.True(stopwatch.ElapsedMilliseconds <= expectedMillisecondsTotal, $"Expected a maximum of {expectedMillisecondsTotal} but took {stopwatch.ElapsedMilliseconds} to add {dataCount} items.");

		    var created = ControllerLocator.GameDataController.Get(gameId, actorId, key);
            Assert.Equal(dataCount, created.Count);
	    }
    }
}
