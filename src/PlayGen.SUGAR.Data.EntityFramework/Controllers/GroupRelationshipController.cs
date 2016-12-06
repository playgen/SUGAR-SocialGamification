using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GroupRelationshipController : DbController
	{
		public GroupRelationshipController(SUGARContextFactory contextFactory) 
			: base(contextFactory)
		{
		}

		public List<User> GetRequests(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var requestors = context.UserToGroupRelationshipRequests
					.Where(r => r.AcceptorId == id)
					.Select(u => u.Requestor).ToList();

				return requestors;
			}
		}

		public List<Group> GetSentRequests(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var acceptors = context.UserToGroupRelationshipRequests
					.Where(r => r.RequestorId == id)
					.Select(u => u.Acceptor).ToList();

				return acceptors;
			}
		}

		public List<User> GetMembers(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var requestors = context.UserToGroupRelationships
					.Where(r => r.AcceptorId == id)
					.Select(u => u.Requestor).ToList();

				return requestors;
			}
		}

		public int GetMemberCount(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var count = context.UserToGroupRelationships
					.Count(r => r.AcceptorId == id);

				return count;
			}
		}

		public List<Group> GetUserGroups(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var acceptors = context.UserToGroupRelationships
					.Where(r => r.RequestorId == id)
					.Select(u => u.Acceptor).ToList();

				return acceptors;
			}
		}

		// todo move auto accept logic into core controller
		public void Create(UserToGroupRelationship newRelation, bool autoAccept)
		{
			using (var context = ContextFactory.Create())
			{
				var hasConflicts = context.UserToGroupRelationships
					.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId) 
					|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));

				if (!hasConflicts)
				{
					hasConflicts = context.UserToGroupRelationshipRequests
						.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId)
						|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));
				}

				if (hasConflicts)
				{
					throw new DuplicateRecordException("A relationship with this user and group already exists.");
				}

				var requestorExists = context.Users.Any(u => u.Id == newRelation.RequestorId);
				var acceptorExists = context.Groups.Any(g => g.Id == newRelation.AcceptorId);

				if (!requestorExists) {
					throw new MissingRecordException("The requesting user does not exist.");
				}

				if (!acceptorExists) {
					throw new MissingRecordException("The targeted group does not exist.");
				}

				if (autoAccept) {
					var relation = new UserToGroupRelationship
					{
						RequestorId = newRelation.RequestorId,
						AcceptorId = newRelation.AcceptorId
					};
					context.UserToGroupRelationships.Add(relation);
				} else {
					var relation = new UserToGroupRelationshipRequest
					{
						RequestorId = newRelation.RequestorId,
						AcceptorId = newRelation.AcceptorId
					};
					context.UserToGroupRelationshipRequests.Add(relation);
				}
				SaveChanges(context);
			}
		}

		// todo move auto accept logic into core controller
		public void UpdateRequest(UserToGroupRelationship newRelation, bool accepted)
		{
			using (var context = ContextFactory.Create())
			{
				var relation = context.UserToGroupRelationshipRequests
					.Single(r => r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId);

				if (accepted)
				{
					var acceptedRelation = new UserToGroupRelationship
					{
						RequestorId = relation.RequestorId,
						AcceptorId = relation.AcceptorId
					};
					context.UserToGroupRelationships.Add(acceptedRelation);
				}
				context.UserToGroupRelationshipRequests.Remove(relation);
				SaveChanges(context);
			}
		}

		public void Update(UserToGroupRelationship newRelation)
		{
			using (var context = ContextFactory.Create())
			{
				var relation = context.UserToGroupRelationships
					.Single(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId) 
						|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));

				context.UserToGroupRelationships.Remove(relation);
				SaveChanges(context);
			}
		}
	}
}