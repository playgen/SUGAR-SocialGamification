using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class GroupController : ActorController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.GroupController _groupDbController;
		private readonly ActorClaimController _actorClaimController;
		private readonly ActorRoleController _actorRoleController;
		private readonly RelationshipController _relationshipController;

		public GroupController(
			ILogger<GroupController> logger,
			EntityFramework.Controllers.GroupController groupDbController,
			EntityFramework.Controllers.ActorController actorDbController,
			ActorClaimController actorClaimController,
			ActorRoleController actorRoleController,
			RelationshipController relationshipController) : base(actorDbController)
		{
			_logger = logger;
			_groupDbController = groupDbController;
			_actorClaimController = actorClaimController;
			_actorRoleController = actorRoleController;
			_relationshipController = relationshipController;
		}

		public List<Group> Get()
		{
			var groups = _groupDbController.Get();
			groups.ForEach(g => g.UserRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.User));
			groups.ForEach(g => g.GroupRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.Group));

			_logger.LogInformation($"{groups?.Count} Groups");

			return groups;
		}

		public List<Group> GetByPermissions(int actorId)
		{
			var groups = Get();
			var permissions = _actorClaimController.GetActorClaimsByScope(actorId, ClaimScope.Group).Select(p => p.EntityId).ToList();
			groups = groups.Where(g => permissions.Contains(g.Id)).ToList();
			groups.ForEach(g => g.UserRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.User));
			groups.ForEach(g => g.GroupRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.Group));

			_logger.LogInformation($"{groups.Count} Groups");

			return groups;
		}

		public new Group Get(int id)
		{
			var group = _groupDbController.Get(id);

			if (group != null)
			{
				group.UserRelationshipCount = _relationshipController.GetRelationshipCount(group.Id, ActorType.User);
				group.GroupRelationshipCount = _relationshipController.GetRelationshipCount(group.Id, ActorType.Group);
			}

			_logger.LogInformation($"Group: {group?.Id} for Id: {id}");

			return group;
		}

		public List<Group> Search(string name)
		{
			var groups = _groupDbController.Get(name);
			groups.ForEach(g => g.UserRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.User));
			groups.ForEach(g => g.GroupRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.Group));


			_logger.LogInformation($"{groups?.Count} Groups for Name: {name}");

			return groups;
		}

		public Group Create(Group newGroup, int creatorId)
		{
			newGroup = _groupDbController.Create(newGroup);
			_actorRoleController.Create(ClaimScope.Group, creatorId, newGroup.Id);
			_relationshipController.CreateRequest(new ActorRelationship { RequestorId = creatorId, AcceptorId = newGroup.Id }, true);

			_logger.LogInformation($"{newGroup.Id} for CreatorId: {creatorId}");

			return newGroup;
		}

		public void Update(Group group)
		{
			_groupDbController.Update(group);

			_logger.LogInformation($"{group?.Id}");
		}

		public void Delete(int id)
		{
			TriggerDeletedEvent(id);

			_groupDbController.Delete(id);

			_logger.LogInformation($"{id}");
		}
	}
}