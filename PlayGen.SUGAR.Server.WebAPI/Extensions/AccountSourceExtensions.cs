using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class AccountSourceExtensions
	{
		public static AccountSourceResponse ToContract(this AccountSource sourceModel)
		{
			if (sourceModel == null)
			{
				return null;
			}

			return new AccountSourceResponse {
				Id = sourceModel.Id,
				Description = sourceModel.Description,
				Token = sourceModel.Token,
				RequiresPassword = sourceModel.RequiresPassword,
				AutoRegister = sourceModel.AutoRegister
			};
		}

		public static IEnumerable<AccountSourceResponse> ToContractList(this IEnumerable<AccountSource> sourceModels)
		{
			return sourceModels.Select(ToContract).ToList();
		}

		public static AccountSource ToModel(this AccountSourceRequest sourceContract)
		{
			return new AccountSource {
				Description = sourceContract.Description,
				Token = sourceContract.Token,
				RequiresPassword = sourceContract.RequiresPassword.Value,
				AutoRegister = sourceContract.AutoRegister.Value
			};
		}

	}
}
