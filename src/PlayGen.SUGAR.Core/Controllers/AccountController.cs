using PlayGen.SUGAR.Core.Exceptions;
using PlayGen.SUGAR.Data.Model;
using System.Linq;

using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Core.Utilities;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class AccountController
	{
		private readonly Data.EntityFramework.Controllers.AccountController _accountDbController;
		private readonly Data.EntityFramework.Controllers.UserController _userDbController;
	    private readonly UserController _userCoreController;
        private readonly ActorRoleController _actorRoleController;

        // todo only take in account db controller but use core user controller
        public AccountController(Data.EntityFramework.Controllers.AccountController accountDbController,
			        Data.EntityFramework.Controllers.UserController userDbController,
                    UserController userCoreController,
                    ActorRoleController actorRoleController)
		{
			_accountDbController = accountDbController;
			_userDbController = userDbController;
		    _userCoreController = userCoreController;
            _actorRoleController = actorRoleController;
        }

        public Account Login(Account toVerify)
        {
            Account verified;

			var found = _accountDbController.Get(new[] { toVerify.Name }).SingleOrDefault();

			if (found != null && PasswordEncryption.Verify(toVerify.Password, found.Password))
			{
			    verified = found;
			}
			else
			{
                throw new InvalidAccountDetailsException("Invalid Login Details.");
            }

            return verified;
        }
		
		public Account Register(Account toRegister)
		{
		    if(string.IsNullOrWhiteSpace(toRegister.Name) || string.IsNullOrWhiteSpace(toRegister.Password))
		    {
		        throw new InvalidAccountDetailsException("Invalid username or password.");
		    }

		    var user = _userCoreController.Search(toRegister.Name, true).FirstOrDefault() ?? _userDbController.Create(new User
		               {
		                   Name = toRegister.Name
		               });

		    var registered = _accountDbController.Create(new Account
		    {
                Name = toRegister.Name,
                Password = PasswordEncryption.Encrypt(toRegister.Password),
                UserId = user.Id,
                User = user
            });

            _actorRoleController.Create(ClaimScope.Account.ToString(), registered.UserId, registered.Id);

            return registered;
		}

		public void Delete(int id)
		{
			_accountDbController.Delete(id);
		}
	}
}