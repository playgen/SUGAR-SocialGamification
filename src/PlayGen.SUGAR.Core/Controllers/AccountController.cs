using System;
using PlayGen.SUGAR.Core.Exceptions;
using PlayGen.SUGAR.Data.Model;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Core.Utilities;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class AccountController : IAccountController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Data.EntityFramework.Controllers.AccountController _accountDbController;
		private readonly AccountSourceController _accountSourceCoreController;
		private readonly UserController _userCoreController;
		private readonly ActorRoleController _actorRoleController;

		// TODO: only take in account db controller but use core user controller
		public AccountController(Data.EntityFramework.Controllers.AccountController accountDbController,
					AccountSourceController accountSourceCoreController,
					UserController userCoreController,
					ActorRoleController actorRoleController)
		{
			_accountDbController = accountDbController;
			_accountSourceCoreController = accountSourceCoreController;
			_userCoreController = userCoreController;
			_actorRoleController = actorRoleController;
		}

		public Account Authenticate(Account toVerify, string sourceToken)
		{
			Account account;
			AccountSource accountSource;

			if (_accountSourceCoreController.TryGet(sourceToken, out accountSource))
			{
				if (_accountDbController.TryGet(toVerify.Name, accountSource.Id, out account))
				{
					if (accountSource.RequiresPassword)
					{
						if (PasswordEncryption.Verify(toVerify.Password, account.Password) == false)
						{
							throw new InvalidAccountDetailsException("Invalid Login Details.");
						}
					}

					Logger.Info($"Account: {toVerify?.Id} passed verification: {account}");

					return account;
				}

				if (accountSource.AutoRegister)
				{
					return Create(toVerify, sourceToken);
				}
				throw new InvalidAccountDetailsException("Invalid Login Details.");
			}
			throw new InvalidAccountDetailsException("Invalid Login Details.");
		}

		public Account Create(Account toRegister, string sourceToken)
		{
			AccountSource source;

			if (_accountSourceCoreController.TryGet(sourceToken, out source))
			{

				var usernameRegex = string.IsNullOrWhiteSpace(source.UsernameRegex) ? null : new Regex(source.UsernameRegex);

				//TODO: come up with a better validation method
				if (string.IsNullOrWhiteSpace(toRegister.Name) 
					|| (source.RequiresPassword && string.IsNullOrWhiteSpace(toRegister.Password))
					|| (usernameRegex != null && usernameRegex.IsMatch(toRegister.Name) == false))
				{
					throw new InvalidAccountDetailsException("Invalid username or password.");
				}

				var user = _userCoreController.Search(toRegister.Name, true).FirstOrDefault() 
					?? _userCoreController.Create(new User { Name = toRegister.Name });

				if (source.RequiresPassword)
				{
					toRegister.Password = PasswordEncryption.Encrypt(toRegister.Password);
				}

				var registered = _accountDbController.Create(new Account
				{
					Name = toRegister.Name,
					Password = toRegister.Password,
					AccountSourceId = source.Id,
					UserId = user.Id,
					User = user
				});

				_actorRoleController.Create(ClaimScope.Account.ToString(), registered.UserId, registered.Id);

				Logger.Info($"{registered?.Id}");

				return registered;
			}
			throw new InvalidAccountDetailsException($"Authentication source {sourceToken} not found.");
		}

		public void Delete(int id)
		{
			//_accountDbController.Delete(id);



			Logger.Info($"{id}");
		}
	}
}