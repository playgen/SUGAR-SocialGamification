using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GroupControllerTests
	{
		#region Configuration
		private readonly GroupController _groupDbController;

		public GroupControllerTests()
		{
			_groupDbController = TestEnvironment.GroupController;
		}
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetGroup()
		{
			var groupName = "CreateGroup";

			CreateGroup(groupName);

			var groups = _groupDbController.Search(groupName);

			var matches = groups.Count(g => g.Name == groupName);

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateDuplicateGroup()
		{
			var groupName = "CreateDuplicateGroup";

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
		public void GetGroupById()
		{
			var newGroup = CreateGroup("GetGroupById");

			var id = newGroup.Id;

			var group = _groupDbController.Search(id);

			Assert.NotNull(group);
			Assert.Equal(newGroup.Name, group.Name);
		}

		[Fact]
		public void GetNonExistingGroupById()
		{
			var group = _groupDbController.Search(-1);

			Assert.Null(group);
		}

		[Fact]
		public void UpdateGroup()
		{
			var groupName = "UpdateExistingGroup";

			var newGroup = CreateGroup(groupName);

			var groups = _groupDbController.Search(groupName);

			var matches = groups.Count(g => g.Name == groupName);

			Assert.Equal(1, matches);

			var updateGroup = new Group
			{
				Id = newGroup.Id,
				Name = "UpdateExistingGroupProof"
			};

			_groupDbController.Update(updateGroup);

			var updatedGroup = _groupDbController.Search(newGroup.Id);

			Assert.Equal("UpdateExistingGroupProof", updatedGroup.Name);
		}

		[Fact]
		public void UpdateGroupToDuplicateName()
		{
			var groupName = "UpdateGroupToDuplicateName";

			var newGroup = CreateGroup(groupName);

			var newGroupDuplicate = CreateGroup(groupName + " Two");

			var updateGroup = new Group
			{
				Id = newGroupDuplicate.Id,
				Name = newGroup.Name
			};

			Assert.Throws<DuplicateRecordException>(() => _groupDbController.Update(updateGroup));
		}

		[Fact]
		public void UpdateNonExistingGroup()
		{
			var group = new Group
			{
				Id = -1,
				Name = "UpdateNonExistingGroup"
			};

			Assert.Throws<MissingRecordException>(() => _groupDbController.Update(group));
		}

		[Fact]
		public void DeleteExistingGroup()
		{
			var groupName = "DeleteExistingGroup";

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