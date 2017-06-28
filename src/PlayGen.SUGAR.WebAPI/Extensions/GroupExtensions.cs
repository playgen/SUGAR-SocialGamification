using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class GroupExtensions
	{
		public static GroupResponse ToContract(this Group groupModel)
		{
			if (groupModel == null)
				return null;
			var groupContract = new GroupResponse
			{
				Id = groupModel.Id,
				Name = groupModel.Name,
				MemberCount = groupModel.UserToGroupRelationships?.Count ?? 0
			};

			return groupContract;
		}

		public static GroupsResponse ToCollectionContract(this IEnumerable<Group> models)
		{
			return new GroupsResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}

		public static Group ToGroupModel(this GroupRequest groupContract)
		{
			return new Group
			{
				Name = groupContract.Name
			};
		}
	}
}