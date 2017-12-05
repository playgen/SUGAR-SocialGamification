using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class UserController : ActorController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.UserController _userController;
		private readonly ActorRoleController _actorRoleController;

		public UserController(
			ILogger<UserController> logger,
			EntityFramework.Controllers.UserController userController,
			EntityFramework.Controllers.ActorController actorDbController,
			ActorRoleController actorRoleController) : base(actorDbController)
		{
			_logger = logger;
			_userController = userController;
			_actorRoleController = actorRoleController;
		}

		public List<User> Get()
		{
			var users = _userController.Get();

			_logger.LogInformation($"{users?.Count} Users");

			return users;
		}

		public new User Get(int id)
		{
			var user = _userController.Get(id);

			_logger.LogInformation($"User: {user?.Id} for Id: {id}");

			return user;
		}

		public List<User> Search(string name, bool exactMatch)
		{
			var users = _userController.Search(name, exactMatch);

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

		public void Update(User user)
		{
			_userController.Update(user);

			_logger.LogInformation($"{user.Id}");
		}

		public void Delete(int id)
		{
			TriggerDeletedEvent(id);

			_userController.Delete(id);

			_logger.LogInformation($"{id}");
		}
	}
}