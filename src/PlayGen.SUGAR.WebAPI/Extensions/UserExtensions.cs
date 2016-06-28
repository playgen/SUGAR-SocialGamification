using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class UserExtensions
	{

		public static ActorResponse ToContract(this User userModel)
		{
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
			var actorModel = new User { Name = actorContract.Name };

			return actorModel;
		}
	}
}