using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public interface IAccountController
	{
		Account Authenticate(Account toVerify, string sourceToken);
		Account Create(Account toRegister, string sourceToken);
		void Delete(int id);
	}
}