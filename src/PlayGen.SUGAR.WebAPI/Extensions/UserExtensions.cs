using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class UserExtensions
	{

		public static ActorResponse ToContract(this User userModel)
		{
			if (userModel == null)
			{
				return null;
			}
			var actorContract = new ActorResponse
			{
				Id = userModel.Id,
				Name = userModel.Name
			};

			return actorContract;
		}

		public static IEnumerable<ActorResponse> ToContractList(this IEnumerable<User> userModels)
		{
			return userModels.Select(ToContract).ToList();
		}

		public static User ToUserModel(this ActorRequest actorContract)
		{
			return new User
			{
			    Name = actorContract.Name
			};
		}
	}
}