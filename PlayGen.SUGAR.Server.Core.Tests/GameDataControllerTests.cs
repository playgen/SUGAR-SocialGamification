using System;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.Core.Tests
{
    public class GameDataControllerTests : CoreTestBase
    {
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
	}
}
