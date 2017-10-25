using System.Collections.Generic;
using NLog;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class AccountSourceController
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly EntityFramework.Controllers.AccountSourceController _accountSourceDbController;

		public AccountSourceController(EntityFramework.Controllers.AccountSourceController accountSourceDbController)
		{
			_accountSourceDbController = accountSourceDbController;
		}

		public List<AccountSource> Get()
		{
			var sources = _accountSourceDbController.Get();

			Logger.Info($"{sources?.Count} Sources.");

			return sources;
		}

		public AccountSource Get(int id)
		{
			var source = _accountSourceDbController.Get(id);

			Logger.Info($"Source: {source?.Id} for Id: {id}");

			return source;
		}

		public AccountSource GetByToken(string token)
		{
			var source = _accountSourceDbController.Get(token);

			Logger.Info($"Source: {source?.Id} for Token: {token}");

			return source;
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
