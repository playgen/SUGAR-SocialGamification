using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class UserController : ActorController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.UserController _userController;
		private readonly ActorRoleController _actorRoleController;
		private readonly RelationshipController _relationshipController;
		private readonly GroupController _groupController;

		public UserController(
			ILogger<UserController> logger,
			EntityFramework.Controllers.UserController userController,
			EntityFramework.Controllers.ActorController actorDbController,
			ActorRoleController actorRoleController,
			RelationshipController relationshipController,
			GroupController groupController) : base(actorDbController)
		{
			_logger = logger;
			_userController = userController;
			_actorRoleController = actorRoleController;
			_relationshipController = relationshipController;
			_groupController = groupController;

		}

		public List<User> Get()
		{
			var users = _userController.Get();
			users.ForEach(u => u.UserRelationshipCount = _relationshipController.GetRelationshipCount(u.Id, ActorType.User));
			users.ForEach(u => u.GroupRelationshipCount = _relationshipController.GetRelationshipCount(u.Id, ActorType.Group));

			_logger.LogInformation($"{users?.Count} Users");

			return users;
		}

		public new User Get(int id)
		{
			var user = _userController.Get(id);

			if (user != null)
			{
				user.UserRelationshipCount = _relationshipController.GetRelationshipCount(user.Id, ActorType.User);
				user.GroupRelationshipCount = _relationshipController.GetRelationshipCount(user.Id, ActorType.Group);
			}

			_logger.LogInformation($"User: {user?.Id} for Id: {id}");

			return user;
		}

		public List<User> Search(string name, bool exactMatch)
		{
			var users = _userController.Search(name, exactMatch);
			users.ForEach(u => u.UserRelationshipCount = _relationshipController.GetRelationshipCount(u.Id, ActorType.User));
			users.ForEach(u => u.GroupRelationshipCount = _relationshipController.GetRelationshipCount(u.Id, ActorType.Group));

			_logger.LogInformation($"{users?.Count} Users for Name: {name}, ExactMatch: {exactMatch}");

			return users;
		}

		public User Create(User newUser, SUGARContext context = null)
		{
			newUser = _userController.Create(newUser, context);

			_actorRoleController.Create(ClaimScope.User, newUser.Id, newUser.Id, context);

			_logger.LogInformation($"{newUser.Id}");

			return newUser;
		}

		public User Update(User user)
		{
			_logger.LogInformation($"{user.Id}");

			return _userController.Update(user);
		}

		public void Delete(int id)
		{
			TriggerDeleteEvent(id);

            // get all groups where this user is the only admin
            // delete all returned groups that only have this user as the member
			var groups = _relationshipController.GetRelatedActors(id, ActorType.Group);
			groups.ForEach(g => _groupController.RemoveMember(g.Id, id));

            _userController.Delete(id);

			_logger.LogInformation($"{id}");
		}
	}
}