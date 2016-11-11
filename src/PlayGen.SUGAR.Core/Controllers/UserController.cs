using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class UserController
    {
        private readonly Data.EntityFramework.Controllers.UserController _userController;

        public UserController(Data.EntityFramework.Controllers.UserController userController)
        {
            _userController = userController;
        }
        
        public IEnumerable<User> Get()
        {
            var users = _userController.Get();
            return users;
        }
        
        public IEnumerable<User> Get(string name, bool exactMatch)
        {
            var users = _userController.Search(name, exactMatch);
            return users;
        }
        
        public User Get(int id)
        {
            var user = _userController.Search(id);
            return user;
        }
        
        public User Create(User newUser)
        {
            newUser = _userController.Create(newUser);
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