using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Core.Extensions;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class UserController : ActorController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.UserController _userController;
		private readonly ActorRoleController _actoRoleController;
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
			_actoRoleController = actorRoleController;
			_relationshipController = relationshipController;
			_groupController = groupController;

		}

		/// <summary>
		/// Get a list of all the users
		/// </summary>
		/// <param name="requestingid">Id of the requesting actor, used to check if the actor has permissions to get the list with private members included </param>
		/// <returns></returns>
		public List<User> GetAll(ActorVisibilityFilter actorVisibilityFilter)
		{
			var users = _userController.Get();
			users = users.FilterVisibility(actorVisibilityFilter).ToList();

			users.ForEach(u => u.UserRelationshipCount = _relationshipController.GetRelationshipCount(u.Id, ActorType.User));
			users.ForEach(u => u.GroupRelationshipCount = _relationshipController.GetRelationshipCount(u.Id, ActorType.Group));

			_logger.LogInformation($"{users?.Count} Users");

			return users;
		}

		public User Get(int id, ActorVisibilityFilter actorVisibilityFilter)
		{
			var user = _userController.Get(id);
			user = user.FilterVisibility(actorVisibilityFilter);

			if (user == null)
			{
				return null;
			}

			user.UserRelationshipCount = _relationshipController.GetRelationshipCount(user.Id, ActorType.User);
			user.GroupRelationshipCount = _relationshipController.GetRelationshipCount(user.Id, ActorType.Group);

			_logger.LogInformation($"User: {user?.Id} for Id: {id}");

			return user;
		}

		public User GetExistingUser(string name)
		{
			return _userController.Search(name, true).FirstOrDefault();
		}

		public List<User> Search(string name, bool exactMatch, ActorVisibilityFilter actorVisibilityFilter)
		{
			var users = _userController.Search(name, exactMatch);
			users = users.FilterVisibility(actorVisibilityFilter).ToList();

			users.ForEach(u => u.UserRelationshipCount = _relationshipController.GetRelationshipCount(u.Id, ActorType.User));
			users.ForEach(u => u.GroupRelationshipCount = _relationshipController.GetRelationshipCount(u.Id, ActorType.Group));

			_logger.LogInformation($"{users?.Count} Users for Name: {name}, ExactMatch: {exactMatch}");

			return users;
		}

		public User Create(User newUser, SUGARContext context = null)
		{
			newUser = _userController.Create(newUser, context);

			_actoRoleController.Create(ClaimScope.User, newUser.Id, newUser.Id, context);

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