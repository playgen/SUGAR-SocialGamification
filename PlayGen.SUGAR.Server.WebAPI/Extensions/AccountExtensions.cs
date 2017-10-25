using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	public static class AccountExtensions
	{
		public static AccountResponse ToContract(this Account model)
		{
			if (model == null)
			{
				return null;
			}

			return new AccountResponse
			{
				User = model.User.ToContract()
			};
		}

		public static Account ToModel(this AccountRequest contract)
		{
			return new Account
			{
				Name = contract.Name,
				Password = contract.Password,
			};
		}
	}
}