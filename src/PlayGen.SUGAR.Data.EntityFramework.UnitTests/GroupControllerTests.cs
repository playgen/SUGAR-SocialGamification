using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GroupControllerTests : TestController
	{
		#region Configuration

		private readonly GroupController _groupDbController;

		public GroupControllerTests()
		{
				_groupDbController = new GroupController(NameOrConnectionString);
		}

		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetGroup()
		{
			const string groupName = "CreateGroup";
			CreateGroup(groupName);
			var groups = _groupDbController.Search(groupName);

			var matches = groups.Count(g => g.Name == groupName);
			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateDuplicateGroup()
		{
			const string groupName = "CreateDuplicateGroup";
			CreateGroup(groupName);

			Assert.Throws<DuplicateRecordException>(() => CreateGroup(groupName));
		}

		[Fact]
		public void GetMultipleGroups()
		{
			var groupNames = new[]
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
			Assert.NotNull(groups);
			Assert.Equal(groups.Count(), 1);
			Assert.Equal(groups.ElementAt(0).Name, groupName);

			_groupDbController.Delete(group.Id);
			groups = _groupDbController.Search(groupName);

			Assert.Empty(groups);
		}

		[Fact]
		public void DeleteNonExistingGroup()
		{
			//TODO: make exception type specific
			Assert.Throws<Exception>(() => _groupDbController.Delete(-1));
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
