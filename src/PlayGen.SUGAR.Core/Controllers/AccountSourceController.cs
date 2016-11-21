using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class AccountSourceController
    {
        private readonly Data.EntityFramework.Controllers.AccountSourceController _accountSourceDbController;

        public AccountSourceController(Data.EntityFramework.Controllers.AccountSourceController accountSourceDbController)
        {
            _accountSourceDbController = accountSourceDbController;
        }

        public IEnumerable<AccountSource> Get()
        {
            var sources = _accountSourceDbController.Get();
            return sources;
        }

        public AccountSource Get(int id)
        {
            var source = _accountSourceDbController.Get(id);
            return source;
        }

        public AccountSource Create(AccountSource newSource)
        {
            newSource = _accountSourceDbController.Create(newSource);
            return newSource;
        }

        public void Update(AccountSource game)
        {
            _accountSourceDbController.Update(game);
        }

        public void Delete(int id)
        {
            _accountSourceDbController.Delete(id);
        }
    }
}
