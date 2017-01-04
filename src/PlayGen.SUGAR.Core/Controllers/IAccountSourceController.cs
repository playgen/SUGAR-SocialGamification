using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public interface IAccountSourceController
	{
		List<AccountSource> Get();
		bool TryGet(int id, out AccountSource accountSource);
		bool TryGet(string token, out AccountSource accountSource);
		AccountSource Create(AccountSource newSource);
		void Update(AccountSource source);
		void Delete(int id);
	}
}