using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Core.Extensions;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class GroupController : ActorController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.GroupController _groupDbController;
		private readonly ActorRoleController _actorRoleController;
		private readonly RelationshipController _relationshipController;

		public GroupController(
			ILogger<GroupController> logger,
			EntityFramework.Controllers.GroupController groupDbController,
			EntityFramework.Controllers.ActorController actorDbController,
			ActorRoleController actorRoleController,
			RelationshipController relationshipController) : base(actorDbController)
		{
			_logger = logger;
			_groupDbController = groupDbController;
			_actorRoleController = actorRoleController;
			_relationshipController = relationshipController;
		}

		public List<Group> GetAll(ActorVisibilityFilter actorVisibilityFilter)
		{
			var groups = _groupDbController.Get();
			
			groups = groups.FilterVisibility(actorVisibilityFilter).ToList();

			groups.ForEach(g => g.UserRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.User));
			groups.ForEach(g => g.GroupRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.Group));

			_logger.LogInformation($"{groups?.Count} Groups");

			return groups;
		}

		public Group Get(int id, ActorVisibilityFilter actorVisibilityFilter = ActorVisibilityFilter.Public)
		{
			var group = _groupDbController.Get(id);
			group = group.FilterVisibility(actorVisibilityFilter);

			if (group != null)
			{
				group.UserRelationshipCount = _relationshipController.GetRelationshipCount(group.Id, ActorType.User);
				group.GroupRelationshipCount = _relationshipController.GetRelationshipCount(group.Id, ActorType.Group);
			}

			_logger.LogInformation($"Group: {group?.Id} for Id: {id}");

			return group;
		}

        public List<Group> GetControlled(int userId, ActorVisibilityFilter actorVisibilityFilter = ActorVisibilityFilter.All)
		{
			var groups = GetAll(actorVisibilityFilter);

			var groupsWhereControlled = groups
				.Where(g => IsGroupController(userId, g.Id))
				.ToList();
				
			groups.ForEach(g =>
			{
				g.UserRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.User);
				g.GroupRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.Group);
			});

			_logger.LogInformation($"{groups.Count} Groups");

			return groupsWhereControlled;
		}

		/// <summary>
        /// Cleanly remove a group member. If it is the admin, admin will be delegated to another member.
        /// If the only member, the group will be deleted.
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="memberId"></param>
		public void RemoveMember(int groupId, int memberId)
		{
			var members = _relationshipController.GetRelatedActors(groupId, ActorType.User, ActorVisibilityFilter.All);

			if (members.All(m => m.Id != memberId))
			{
				throw new InvalidRelationshipException($"The actor with id {memberId} is not a member of the group: {groupId}.");
			}

			var relationship = _relationshipController.GetRelationship(groupId, memberId);

			if (members.Count == 1)
			{
				_relationshipController.Delete(relationship);
				Delete(groupId);
			}
			else
			{
				var groupControllers = members
					.Where(m => IsGroupController(m.Id, groupId))
					.ToList();
				
                // If this is the only group controller, assign control to another member
                if (groupControllers.Count == 1 && groupControllers.Any(gc => gc.Id == memberId))
				{
					_relationshipController.Delete(relationship);

                    var relationships = _relationshipController.GetRelationships(groupId, ActorType.User);
					var newControllerRelationship = relationships.First();
					var newControllerId = newControllerRelationship.AcceptorId != groupId
						? newControllerRelationship.AcceptorId
						: newControllerRelationship.RequestorId;
					
                    AssignController(newControllerId, groupId);
				}
				else
				{
					_relationshipController.Delete(relationship);
                }
			}
		}

		public List<Group> Search(string name, ActorVisibilityFilter actorVisibilityFilter = ActorVisibilityFilter.Public)
		{
			var groups = _groupDbController.Get(name);
			groups = groups.FilterVisibility(actorVisibilityFilter).ToList();

			groups.ForEach(g => g.UserRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.User));
			groups.ForEach(g => g.GroupRelationshipCount = _relationshipController.GetRelationshipCount(g.Id, ActorType.Group));


			_logger.LogInformation($"{groups?.Count} Groups for Name: {name}");

			return groups;
		}

		public Group Create(Group newGroup, int creatorId)
		{
			newGroup = _groupDbController.Create(newGroup);

			AssignController(creatorId, newGroup.Id);
			_relationshipController.CreateRequest(new ActorRelationship { RequestorId = creatorId, AcceptorId = newGroup.Id }, true);

			_logger.LogInformation($"{newGroup.Id} for CreatorId: {creatorId}");

			return newGroup;
		}

		private void AssignController(int controllerId, int groupId)
		{
			_actorRoleController.Create(ClaimScope.Group, controllerId, groupId);
        }

		public void Update(Group group)
		{
			_groupDbController.Update(group);

			_logger.LogInformation($"{group?.Id}");
		}

		public void Delete(int id)
		{
			TriggerDeleteEvent(id);

			_groupDbController.Delete(id);

			_logger.LogInformation($"{id}");
		}

		private bool IsGroupController(int memberId, int groupId)
		{
			// Get members of group where they have a role over the entire group (claim scope) 
			// and that role has the "Default" property which means it is an auto generated role.
			// This is the role that is given to the creator of the group and can be used to prove that it is the admin (the group controller).
			
			return _actorRoleController
					.GetActorRolesForEntity(memberId, groupId, ClaimScope.Group)
					.Any(r => r.Default);
		}
	}
}