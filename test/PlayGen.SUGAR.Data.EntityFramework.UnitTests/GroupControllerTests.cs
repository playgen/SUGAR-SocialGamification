using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using NUnit.Framework;

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
		[Test]
		public void CreateAndGetGroup()
		{
			var groupName = "CreateGroup";

			CreateGroup(groupName);

			var groups = _groupDbController.Search(groupName);

			var matches = groups.Count(g => g.Name == groupName);

			Assert.AreEqual(matches, 1);
		}

		[Test]
		public void CreateDuplicateGroup()
		{
			var groupName = "CreateDuplicateGroup";

			CreateGroup(groupName);

			Assert.Throws<DuplicateRecordException>(() => CreateGroup(groupName));
		}

		[Test]
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

			Assert.AreEqual(matchingGroups.Count(), groupNames.Length);
		}

		[Test]
		public void GetNonExistingGroup()
		{
			var groups = _groupDbController.Search("GetNonExsitingGroup");

			Assert.IsEmpty(groups);
		}

		[Test]
		public void GetGroupById()
		{
			var newGroup = CreateGroup("GetGroupById");

			var id = newGroup.Id;

			var group = _groupDbController.Search(id);

			Assert.NotNull(group);
			Assert.AreEqual(newGroup.Name, group.Name);
		}

		[Test]
		public void GetNonExistingGroupById()
		{
			var group = _groupDbController.Search(-1);

			Assert.Null(group);
		}

		[Test]
		public void UpdateGroup()
		{
			var groupName = "UpdateExistingGroup";

			var newGroup = CreateGroup(groupName);

			var groups = _groupDbController.Search(groupName);

			var matches = groups.Count(g => g.Name == groupName);

			Assert.AreEqual(1, matches);

			var updateGroup = new Group
			{
				Id = newGroup.Id,
				Name = "UpdateExistingGroupProof"
			};

			_groupDbController.Update(updateGroup);

			var updatedGroup = _groupDbController.Search(newGroup.Id);

			Assert.AreEqual("UpdateExistingGroupProof", updatedGroup.Name);
		}

		[Test]
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

		[Test]
		public void UpdateNonExistingGroup()
		{
			var group = new Group
			{
				Id = -1,
				Name = "UpdateNonExistingGroup"
			};

			Assert.Throws<MissingRecordException>(() => _groupDbController.Update(group));
		}

		[Test]
		public void DeleteExistingGroup()
		{
			var groupName = "DeleteExistingGroup";

			var group = CreateGroup(groupName);

			var groups = _groupDbController.Search(groupName);
			Assert.AreEqual(groups.Count(), 1);
			Assert.AreEqual(groups.ElementAt(0).Name, groupName);

			_groupDbController.Delete(group.Id);
			groups = _groupDbController.Search(groupName);

			Assert.IsEmpty(groups);
		}

		[Test]
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