using System.Collections.Generic;
using NLog;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class AccountSourceController : IAccountSourceController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Data.EntityFramework.Controllers.AccountSourceController _accountSourceDbController;

		public AccountSourceController(Data.EntityFramework.Controllers.AccountSourceController accountSourceDbController)
		{
			_accountSourceDbController = accountSourceDbController;
		}

		public List<AccountSource> Get()
		{
			var sources = _accountSourceDbController.Get();

			Logger.Info($"{sources?.Count} Sources.");

			return sources;
		}

		public bool TryGet(int id, out AccountSource accountSource)
		{
			accountSource = _accountSourceDbController.Get(id);

			Logger.Info($"Source: {accountSource?.Id} for Id: {id}");

			return accountSource != null;
		}

		public bool TryGet(string token, out AccountSource accountSource)
		{
			accountSource = _accountSourceDbController.Get(token);

			Logger.Info($"Source: {accountSource?.Id} for Token: {token}");

			return accountSource != null;
		}

		public AccountSource Create(AccountSource newSource)
		{
			newSource = _accountSourceDbController.Create(newSource);

			Logger.Info($"{newSource?.Id}");

			return newSource;
		}

		public void Update(AccountSource source)
		{
			_accountSourceDbController.Update(source);

			Logger.Info($"{source?.Id}");
		}

		public void Delete(int id)
		{
			_accountSourceDbController.Delete(id);

			Logger.Info($"{id}");
		}
	}
}
