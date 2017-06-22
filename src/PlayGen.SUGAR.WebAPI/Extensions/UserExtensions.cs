using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class UserExtensions
	{

		public static UserResponse ToContract(this User userModel)
		{
			if (userModel == null)
			{
				return null;
			}
			var userContract = new UserResponse {
				Id = userModel.Id,
				Name = userModel.Name
			};

			return userContract;
		}

		public static IEnumerable<UserResponse> ToContractList(this IEnumerable<User> userModels)
		{
			return userModels.Select(ToContract).ToList();
		}

		public static User ToUserModel(this UserRequest userContract)
		{
			return new User {
				Name = userContract.Name
			};
		}
	}
}