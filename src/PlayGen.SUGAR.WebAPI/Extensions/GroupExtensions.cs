using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class GroupExtensions
	{
		public static ActorResponse ToContract(this Group groupModel)
		{
			if (groupModel == null)
			{
				return null;
			}
			var actorContract = new ActorResponse
			{
				Id = groupModel.Id,
				Name = groupModel.Name
			};

			return actorContract;
		}

		public static IEnumerable<ActorResponse> ToContractList(this IEnumerable<Group> groupModels)
		{
			return groupModels.Select(ToContract).ToList();
		}

		public static Group ToGroupModel(this ActorRequest actorContract)
		{
			var actorModel = new Group { Name = actorContract.Name };

			return actorModel;
		}
	}
}