using System.Collections.Generic;
using NLog;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class UserController : ActorController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Data.EntityFramework.Controllers.UserController _userController;
        private readonly ActorRoleController _actorRoleController;

        public UserController(Data.EntityFramework.Controllers.UserController userController,
                    Data.EntityFramework.Controllers.ActorController actorDbController,
                    ActorRoleController actorRoleController) : base(actorDbController)
        {
            _userController = userController;
            _actorRoleController = actorRoleController;
        }
        
        public List<User> Get()
        {
            var users = _userController.Get();

            Logger.Info($"{users?.Count} Users");

            return users;
        }

        public User Get(int id)
        {
            var user = _userController.Get(id);

            Logger.Info($"User: {user?.Id} for Id: {id}");

            return user;
        }

        public List<User> Search(string name, bool exactMatch)
        {
            var users = _userController.Search(name, exactMatch);

            Logger.Info($"{users?.Count} Users for Name: {name}, ExactMatch: {exactMatch}");

            return users;
        }
        
        public User Create(User newUser)
        {
            newUser = _userController.Create(newUser);
            _actorRoleController.Create(ClaimScope.User.ToString(), newUser.Id, newUser.Id);

            Logger.Info($"{newUser.Id}");

            return newUser;
        }
        
        public void Update(User user)
        {
            _userController.Update(user);

            Logger.Info($"{user.Id}");
        }
        
        public void Delete(int id)
        {
            TriggerDeletedEvent(id);

            _userController.Delete(id);

            Logger.Info($"{id}");
        }
    }
}