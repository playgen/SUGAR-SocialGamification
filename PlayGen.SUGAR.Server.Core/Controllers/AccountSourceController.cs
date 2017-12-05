using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class AccountSourceController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.AccountSourceController _accountSourceDbController;

		public AccountSourceController(
			ILogger<AccountSourceController> logger,
			EntityFramework.Controllers.AccountSourceController accountSourceDbController)
		{
			_logger = logger;
			_accountSourceDbController = accountSourceDbController;
		}

		public List<AccountSource> Get()
		{
			var sources = _accountSourceDbController.Get();

			_logger.LogInformation($"{sources?.Count} Sources.");

			return sources;
		}

		public AccountSource Get(int id)
		{
			var source = _accountSourceDbController.Get(id);

			_logger.LogInformation($"Source: {source?.Id} for Id: {id}");

			return source;
		}

		public AccountSource GetByToken(string token)
		{
			var source = _accountSourceDbController.Get(token);

			_logger.LogInformation($"Source: {source?.Id} for Token: {token}");

			return source;
		}

		public AccountSource Create(AccountSource newSource)
		{
			newSource = _accountSourceDbController.Create(newSource);

			_logger.LogInformation($"{newSource?.Id}");

			return newSource;
		}

		public void Update(AccountSource source)
		{
			_accountSourceDbController.Update(source);

			_logger.LogInformation($"{source?.Id}");
		}

		public void Delete(int id)
		{
			_accountSourceDbController.Delete(id);

			_logger.LogInformation($"{id}");
		}
	}
}
