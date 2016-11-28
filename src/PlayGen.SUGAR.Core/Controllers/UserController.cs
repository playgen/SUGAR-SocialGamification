using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class UserController : ActorController
    {
        private readonly Data.EntityFramework.Controllers.UserController _userController;
        private readonly ActorRoleController _actorRoleController;

        public UserController(Data.EntityFramework.Controllers.UserController userController,
                    ActorRoleController actorRoleController)
        {
            _userController = userController;
            _actorRoleController = actorRoleController;
        }
        
        public IEnumerable<User> Get()
        {
            var users = _userController.Get();
            return users;
        }

        public User Get(int id)
        {
            var user = _userController.Get(id);
            return user;
        }

        public IEnumerable<User> Search(string name, bool exactMatch)
        {
            var users = _userController.Search(name, exactMatch);
            return users;
        }
        
        public User Create(User newUser)
        {
            newUser = _userController.Create(newUser);
            _actorRoleController.Create(ClaimScope.User.ToString(), newUser.Id, newUser.Id);
            return newUser;
        }
        
        public void Update(User user)
        {
            _userController.Update(user);
        }
        
        public void Delete(int id)
        {
            TriggerDeletedEvent(id);

            _userController.Delete(id);
        }
    }
}