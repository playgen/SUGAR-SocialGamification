using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GroupControllerTests : IClassFixture<TestEnvironment>
	{
		#region Configuration
		private readonly GroupController _groupDbController;

		public GroupControllerTests(TestEnvironment testEnvironment)
		{
			_groupDbController = testEnvironment.GroupController;
		}
		#endregion


		#region Tests
		[Fact]
		public void CreateAndGetGroup()
		{
			string groupName = "CreateGroup";

			CreateGroup(groupName);

			var groups = _groupDbController.Search(groupName);

			int matches = groups.Count(g => g.Name == groupName);

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateDuplicateGroup()
		{
			string groupName = "CreateDuplicateGroup";

			CreateGroup(groupName);

			Assert.Throws<DuplicateRecordException>(() => CreateGroup(groupName));
		}

		[Fact]
		public void GetMultipleGroups()
		{
			string[] groupNames = new[]
			{
					"GetMultipleGroups1",
					"GetMultipleGroups2",
					"GetMultipleGroups3",
					"GetMultipleGroups4",
				};

			foreach (var groupName in groupNames)
			{
				CreateGroup(groupName);
			}

			CreateGroup("GetMultiple_Groups_DontGetThis");

			var groups = _groupDbController.Search("GetMultipleGroups");

			var matchingGroups = groups.Select(g => groupNames.Contains(g.Name));

			Assert.Equal(matchingGroups.Count(), groupNames.Length);
		}

		[Fact]
		public void GetNonExistingGroup()
		{
			var groups = _groupDbController.Search("GetNonExsitingGroup");

			Assert.Empty(groups);
		}

		[Fact]
		public void DeleteExistingGroup()
		{
			string groupName = "DeleteExistingGroup";

			var group = CreateGroup(groupName);

			var groups = _groupDbController.Search(groupName);
			Assert.Equal(groups.Count(), 1);
			Assert.Equal(groups.ElementAt(0).Name, groupName);

			_groupDbController.Delete(group.Id);
			groups = _groupDbController.Search(groupName);

			Assert.Empty(groups);
		}

		[Fact]
		public void DeleteNonExistingGroup()
		{
			_groupDbController.Delete(-1);
		}
		#endregion

		#region Helpers
		private Group CreateGroup(string name)
		{
			var group = new Group
			{
				Name = name,
			};
			_groupDbController.Create(group);

			return group;
		}
		#endregion
	}
}