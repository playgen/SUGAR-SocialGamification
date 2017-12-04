using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Core.Exceptions;
using PlayGen.SUGAR.Server.Core.Utilities;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class AccountController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.AccountController _accountDbController;
		private readonly AccountSourceController _accountSourceCoreController;
		private readonly UserController _userCoreController;
		private readonly ActorRoleController _actorRoleController;

		// todo only take in account db controller but use core user controller
		public AccountController(
			ILogger<AccountController> logger,
			EntityFramework.Controllers.AccountController accountDbController,
			AccountSourceController accountSourceCoreController,
			UserController userCoreController,
			ActorRoleController actorRoleController)
		{
			_logger = logger;
			_accountDbController = accountDbController;
			_accountSourceCoreController = accountSourceCoreController;
			_userCoreController = userCoreController;
			_actorRoleController = actorRoleController;
		}

		public Account Authenticate(Account toVerify, string sourceToken)
		{
			Account verified;

			var source = _accountSourceCoreController.GetByToken(sourceToken);

			if (source != null)
			{
				var found = _accountDbController.Get(new[] { toVerify.Name }, source.Id).SingleOrDefault();
				if (found != null)
				{
					if (source.RequiresPassword)
					{
						if (PasswordEncryption.Verify(toVerify.Password, found.Password))
						{
							verified = found;
						}
						else
						{
							throw new InvalidAccountDetailsException("Invalid Login Details.");
						}
					}
					else
					{
						verified = found;
					}

					_logger.LogInformation($"Account: {toVerify?.Id} passed verification: {verified}");

					return verified;
				}
				else if (source.AutoRegister)
				{
					return Create(toVerify, sourceToken);
				}
				throw new InvalidAccountDetailsException("Invalid Login Details.");
			}
			throw new InvalidAccountDetailsException("Invalid Login Details.");
		}

		public Account Create(Account toRegister, string sourceToken)
		{
			var source = _accountSourceCoreController.GetByToken(sourceToken);

			if (string.IsNullOrWhiteSpace(toRegister.Name) || (source.RequiresPassword && string.IsNullOrWhiteSpace(toRegister.Password)))
			{
				throw new InvalidAccountDetailsException("Invalid username or password.");
			}

			var user = _userCoreController.Search(toRegister.Name, true).FirstOrDefault() ?? _userCoreController.Create(new User {
				Name = toRegister.Name
			});

			if (source.RequiresPassword)
			{
				toRegister.Password = PasswordEncryption.Encrypt(toRegister.Password);
			}

			var registered = _accountDbController.Create(new Account {
				Name = toRegister.Name,
				Password = toRegister.Password,
				AccountSourceId = source.Id,
				UserId = user.Id,
				User = user
			});

			_actorRoleController.Create(ClaimScope.Account.ToString(), registered.UserId, registered.Id);

			_logger.LogInformation($"{registered?.Id}");

			return registered;
		}

		public void Delete(int id)
		{
			_accountDbController.Delete(id);

			_logger.LogInformation($"{id}");
		}
	}
}