using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class UserController : ActorController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.UserController _userController;
		private readonly ActorRoleController _actorRoleController;
		private readonly RelationshipController _relationshipController;

		public UserController(
			ILogger<UserController> logger,
			EntityFramework.Controllers.UserController userController,
			EntityFramework.Controllers.ActorController actorDbController,
			ActorRoleController actorRoleController,
			RelationshipController relationshipController) : base(actorDbController)
		{
			_logger = logger;
			_userController = userController;
			_actorRoleController = actorRoleController;
			_relationshipController = relationshipController;
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
			user.UserRelationshipCount = _relationshipController.GetRelationshipCount(user.Id, ActorType.User);
			user.GroupRelationshipCount = _relationshipController.GetRelationshipCount(user.Id, ActorType.Group);

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

		public User Create(User newUser)
		{
			newUser = _userController.Create(newUser);
			_actorRoleController.Create(ClaimScope.User.ToString(), newUser.Id, newUser.Id);

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
			TriggerDeletedEvent(id);

			_userController.Delete(id);

			_logger.LogInformation($"{id}");
		}
	}
}