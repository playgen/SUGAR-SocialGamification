using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class UserController
    {
        private readonly Data.EntityFramework.Controllers.UserController _userController;
        private readonly Data.EntityFramework.Controllers.ActorRoleController _actorRoleController;
        private readonly Data.EntityFramework.Controllers.RoleController _roleController;

        public UserController(Data.EntityFramework.Controllers.UserController userController,
                    Data.EntityFramework.Controllers.ActorRoleController actorRoleController,
                    Data.EntityFramework.Controllers.RoleController roleController)
        {
            _userController = userController;
            _actorRoleController = actorRoleController;
            _roleController = roleController;
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
            var role = _roleController.Get(ClaimScope.Actor.ToString());
            if (role != null)
            {
                _actorRoleController.Create(new ActorRole { ActorId = newUser.Id, RoleId = role.Id, EntityId = newUser.Id });
                var admins = _actorRoleController.GetRoleActors(role.Id, 0);
                foreach (var admin in admins)
                {
                    _actorRoleController.Create(new ActorRole { ActorId = admin.Id, RoleId = role.Id, EntityId = newUser.Id });
                }
            }
            return newUser;
        }
        
        public void Update(User user)
        {
            _userController.Update(user);
        }
        
        public void Delete(int id)
        {
            _userController.Delete(id);
        }
    }
}