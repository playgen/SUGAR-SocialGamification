using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts.Shared;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class GroupClientTests
	{
		#region Configuration
		private readonly GroupClient _groupClient;

		public GroupClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_groupClient = testSugarClient.Group;

			Helpers.CreateAndLogin(testSugarClient.Session);
		}
		#endregion

		#region Tests
		[Test]
		public void CanCreateGroup()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CanCreateGroup",
			};

			var response = _groupClient.Create(groupRequest);

			Assert.AreEqual(groupRequest.Name, response.Name);
			Assert.True(response.Id > 0);
		}

		[Test]
		public void CannotCreateDuplicateGroup()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CannotCreateDuplicateGroup",
			};

			_groupClient.Create(groupRequest);

			Assert.Throws<ClientException>(() => _groupClient.Create(groupRequest));
		}

		[Test]
		public void CannotCreateGroupWithNoName()
		{
			var groupRequest = new ActorRequest { };

			Assert.Throws<ClientException>(() => _groupClient.Create(groupRequest));
		}

		[Test]
		public void CanGetGroupsByName()
		{
			var groupRequestOne = new ActorRequest
			{
				Name = "CanGetGroupsByName 1",
			};

			var responseOne = _groupClient.Create(groupRequestOne);

			var groupRequestTwo = new ActorRequest
			{
				Name = "CanGetGroupsByName 2",
			};

			var responseTwo = _groupClient.Create(groupRequestTwo);

			var getGroups = _groupClient.Get("CanGetGroupsByName");

			Assert.AreEqual(2, getGroups.Count());
		}

		[Test]
		public void CannotGetNotExistingGroupByName()
		{
			var getGroups = _groupClient.Get("CannotGetNotExistingGroupByName");

			Assert.IsEmpty(getGroups);
		}

		[Test]
		public void CannotGetGroupByEmptyName()
		{
			Assert.Throws<ClientException>(() => _groupClient.Get(""));
		}

		[Test]
		public void CanGetGroupById()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CanGetGroupById",
			};

			var response = _groupClient.Create(groupRequest);

			var getGroup = _groupClient.Get(response.Id);

			Assert.AreEqual(response.Name, getGroup.Name);
			Assert.AreEqual(groupRequest.Name, getGroup.Name);
		}

		[Test]
		public void CannotGetNotExistingGroupById()
		{
			var getGroup = _groupClient.Get(-1);

			Assert.Null(getGroup);
		}

		[Test]
		public void CanUpdateGroup()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CanUpdateGroup",
			};

			var response = _groupClient.Create(groupRequest);

			var updateRequest = new ActorRequest
			{
				Name = "CanUpdateGroup Updated"
			};

			_groupClient.Update(response.Id, updateRequest);

			var getGroup = _groupClient.Get(response.Id);

			Assert.AreNotEqual(response.Name, updateRequest.Name);
			Assert.AreEqual("CanUpdateGroup Updated", getGroup.Name);
		}

		[Test]
		public void CannotUpdateGroupToDuplicateName()
		{
			var groupRequestOne = new ActorRequest
			{
				Name = "CannotUpdateGroupToDuplicateName 1"
			};

			var responseOne = _groupClient.Create(groupRequestOne);

			var groupRequestTwo = new ActorRequest
			{
				Name = "CannotUpdateGroupToDuplicateName 2"
			};

			var responseTwo = _groupClient.Create(groupRequestTwo);

			var updateGroup = new ActorRequest
			{
				Name = groupRequestOne.Name
			};

			Assert.Throws<ClientException>(() => _groupClient.Update(responseTwo.Id, updateGroup));
		}

		[Test]
		public void CannotUpdateNonExistingGroup()
		{
			var updateGroup = new ActorRequest
			{
				Name = "CannotUpdateNonExistingGroup"
			};

			Assert.Throws<ClientException>(() => _groupClient.Update(-1, updateGroup));
		}

		[Test]
		public void CannotUpdateGroupToNoName()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CannotUpdateGroupToNoName",
			};

			var response = _groupClient.Create(groupRequest);

			var updateRequest = new ActorRequest
			{
			};

			Assert.Throws<ClientException>(() => _groupClient.Update(response.Id, updateRequest));
		}

		[Test]
		public void CanDeleteGroup()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CanDeleteGroup",
			};

			var response = _groupClient.Create(groupRequest);

			var getGroup = _groupClient.Get(response.Id);

			Assert.NotNull(getGroup);

			_groupClient.Delete(response.Id);

			getGroup = _groupClient.Get(response.Id);

			Assert.Null(getGroup);
		}

		[Test]
		public void CannotDeleteNonExistingGroup()
		{
			_groupClient.Delete(-1);
		}


		#endregion
	}
}