using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.Core.Tests
{
    public class GroupControllerTests : CoreTestBase
    {
		private readonly UserController _userController = ControllerLocator.UserController;
		private readonly GroupController _groupController = ControllerLocator.GroupController;
		private readonly RelationshipController _relationshipController = ControllerLocator.RelationshipController;

		public void GetControlledOnlyReturnsController()
		{
			// Arrange
			var prefix = nameof(GetControlledOnlyReturnsController);

			var creator = _userController.Create(new User
			{
				Name = $"{prefix}_CreatorUser"
			});

			var group = _groupController.Create(new Group
			{
				Name = $"{prefix}_Group"
			}, creator.Id);

			var members = Enumerable.Range(0, 10).Select(i =>
			{
				var member = _userController.Create(new User
				{
					Name = $"{prefix}_MemberUser_{i}"
				});

				_relationshipController.CreateRequest(new ActorRelationship
				{
					AcceptorId = group.Id,
					RequestorId = member.Id
				}, true);

				return member;
			}).ToList();

			// Act & Assert
			members.ForEach(m =>
			{
				var controlled = _groupController.GetControlled(m.Id);
				Assert.Empty(controlled);
			});

			var creatorControlled = _groupController.GetControlled(creator.Id);
			Assert.NotEmpty(creatorControlled);
			Assert.Equal(group.Id, creatorControlled[0].Id);
		}

		[Fact]
		public void GroupIsDeletedWhenOnlyMemberIsRemoved()
		{
			// Arrange
			var prefix = nameof(GroupIsDeletedWhenOnlyMemberIsRemoved);

			var user = _userController.Create(new User
			{
				Name = $"{prefix}_User"
			});

			var group = _groupController.Create(new Group
			{
				Name = $"{prefix}_Group"
			}, user.Id);

			// Act
			_groupController.RemoveMember(group.Id, user.Id);

			// Assert
			var gotGroup = _groupController.Get(group.Id);
			Assert.Null(gotGroup);
		}

		[Fact]
		public void GroupIsNotDeletedWhenStandardMemberIsRemoved()
		{
			// Arrange
			var prefix = nameof(GroupIsNotDeletedWhenStandardMemberIsRemoved);

			var creator = _userController.Create(new User
			{
				Name = $"{prefix}_CreatorUser"
            });

			var group = _groupController.Create(new Group
			{
				Name = $"{prefix}_Group"
            }, creator.Id);

			var member = _userController.Create(new User
			{
				Name = $"{prefix}_MemberUser"
			});

			_relationshipController.CreateRequest(new ActorRelationship
			{
				AcceptorId = group.Id,
				RequestorId = member.Id
			}, true);

            // Act
			_groupController.RemoveMember(group.Id, member.Id);
            
			// Assert
			var gotGroup = _groupController.Get(group.Id);
			Assert.NotNull(gotGroup);
        }

		[Fact]
		public void GroupIsNotDeletedWhenCreatorMemberIsRemoved()
		{
			// Arrange
			var prefix = nameof(GroupIsNotDeletedWhenCreatorMemberIsRemoved);

			var creator = _userController.Create(new User
			{
				Name = $"{prefix}_CreatorUser"
            });

			var group = _groupController.Create(new Group
			{
				Name = $"{prefix}_Group"
            }, creator.Id);

			var member = _userController.Create(new User
			{
				Name = $"{prefix}_MemberUser"
			});

			_relationshipController.CreateRequest(new ActorRelationship
			{
				AcceptorId = group.Id,
				RequestorId = member.Id
			}, true);

            // Act
			_groupController.RemoveMember(group.Id, creator.Id);

            // Assert
            var gotGroup = _groupController.Get(group.Id);
			Assert.NotNull(gotGroup);
        }

		[Fact]
		public void GroupControllerIsReassignedWhenCreatorMemberIsRemoved()
		{
			// Arrange
			var prefix = nameof(GroupControllerIsReassignedWhenCreatorMemberIsRemoved);

			var creator = _userController.Create(new User
			{
				Name = $"{prefix}_CreatorUser"
			});

			var group = _groupController.Create(new Group
			{
				Name = $"{prefix}_Group"
			}, creator.Id);

			var member = _userController.Create(new User
			{
				Name = $"{prefix}_MemberUser"
			});

			_relationshipController.CreateRequest(new ActorRelationship
			{
				AcceptorId = group.Id,
				RequestorId = member.Id
			}, true);

            // Act
			_groupController.RemoveMember(group.Id, creator.Id);

            // Assert
            var controllerGroups = _groupController.GetControlled(member.Id);
			Assert.Contains(controllerGroups, cg => cg.Id == group.Id);
        }
    }
}
