using System.Collections.Generic;
using System.Linq;
using NLog;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class GroupController : ActorController
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly EntityFramework.Controllers.GroupController _groupDbController;
		private readonly ActorClaimController _actorClaimController;
		private readonly ActorRoleController _actorRoleController;
		private readonly GroupMemberController _groupMemberController;

		public GroupController(EntityFramework.Controllers.GroupController groupDbController,
					EntityFramework.Controllers.ActorController actorDbController,
					ActorClaimController actorClaimController,
					ActorRoleController actorRoleController,
					GroupMemberController groupMemberController) : base(actorDbController)
		{
			_groupDbController = groupDbController;
			_actorClaimController = actorClaimController;
			_actorRoleController = actorRoleController;
			_groupMemberController = groupMemberController;
		}

		public List<Group> Get()
		{
			var groups = _groupDbController.Get();

			Logger.Info($"{groups?.Count} Groups");

			return groups;
		}

		public List<Group> GetByPermissions(int actorId)
		{
			var groups = Get();
			var permissions = _actorClaimController.GetActorClaimsByScope(actorId, ClaimScope.Group).Select(p => p.EntityId).ToList();
			groups = groups.Where(g => permissions.Contains(g.Id)).ToList();

			Logger.Info($"{groups?.Count} Groups");

			return groups;
		}

		public new Group Get(int id)
		{
			var group = _groupDbController.Get(id);

			Logger.Info($"Group: {group?.Id} for Id: {id}");

			return group;
		}

		public List<Group> Search(string name)
		{
			var groups = _groupDbController.Get(name);

			Logger.Info($"{groups?.Count} Groups for Name: {name}");

			return groups;
		}

		public Group Create(Group newGroup, int creatorId)
		{
			newGroup = _groupDbController.Create(newGroup);
			_actorRoleController.Create(ClaimScope.Group.ToString(), creatorId, newGroup.Id);
			_groupMemberController.CreateMemberRequest(new UserToGroupRelationship { RequestorId = creatorId, AcceptorId = newGroup.Id }, true);

			Logger.Info($"{newGroup?.Id} for CreatorId: {creatorId}");

			return newGroup;
		}

		public void Update(Group group)
		{
			_groupDbController.Update(group);

			Logger.Info($"{group?.Id}");
		}

		public void Delete(int id)
		{
			TriggerDeletedEvent(id);

			_groupDbController.Delete(id);

			Logger.Info($"{id}");
		}
	}
}