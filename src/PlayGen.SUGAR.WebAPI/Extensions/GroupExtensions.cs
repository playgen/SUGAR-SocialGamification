using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class GroupExtensions
	{
		public static GroupResponse ToContract(this Group groupModel)
		{
			if (groupModel == null)
			{
				return null;
			}
			var groupContract = new GroupResponse {
				Id = groupModel.Id,
				Name = groupModel.Name,
				MemberCount = groupModel.UserToGroupRelationships != null ? groupModel.UserToGroupRelationships.Count : 0
			};

			return groupContract;
		}

		public static IEnumerable<GroupResponse> ToContractList(this IEnumerable<Group> groupModels)
		{
			return groupModels.Select(ToContract).ToList();
		}

		public static Group ToGroupModel(this GroupRequest groupContract)
		{
			return new Group {
				Name = groupContract.Name
			};
		}
	}
}