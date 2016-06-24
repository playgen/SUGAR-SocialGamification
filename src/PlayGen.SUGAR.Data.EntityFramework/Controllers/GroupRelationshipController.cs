using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GroupRelationshipController : DbController
	{
		public GroupRelationshipController(string nameOrConnectionString) 
			: base(nameOrConnectionString)
		{
		}

		public IEnumerable<User> GetRequests(int id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var requestors = context.UserToGroupRelationshipRequests
					.Where(r => r.AcceptorId == id)
					.Select(u => u.Requestor).ToList();

				return requestors;
			}
		}

		public IEnumerable<User> GetMembers(int id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var requestors = context.UserToGroupRelationships
					.Where(r => r.AcceptorId == id)
					.Select(u => u.Requestor).ToList();

				return requestors;
			}
		}

		public IEnumerable<Group> GetUserGroups(int id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var acceptors = context.UserToGroupRelationships
					.Where(r => r.RequestorId == id)
					.Select(u => u.Acceptor).ToList();

				return acceptors;
			}
		}

		public void Create(UserToGroupRelationship newRelation, bool autoAccept)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var hasConflicts = context.UserToGroupRelationships.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId) || (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));
				if (!hasConflicts)
				{
					hasConflicts = context.UserToGroupRelationshipRequests.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId) || (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));
				}

				if (hasConflicts)
				{
					throw new DuplicateRecordException(string.Format("A relationship with this user and group already exists."));
				}

				var requestorExists = context.Users.Any(u => u.Id == newRelation.RequestorId);
				var acceptorExists = context.Groups.Any(g => g.Id == newRelation.AcceptorId);

				if (!requestorExists) {
					throw new MissingRecordException(string.Format("The requesting user does not exist."));
				}

				if (!acceptorExists) {
					throw new MissingRecordException(string.Format("The targeted group does not exist."));
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

		public void UpdateRequest(UserToGroupRelationship newRelation, bool accepted)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var relation = context.UserToGroupRelationshipRequests.Single(r => r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId);

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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var relation = context.UserToGroupRelationships
					.Single(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId) 
						|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));

				context.UserToGroupRelationships.Remove(relation);
				SaveChanges(context);
			}
		}
	}
}