using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class UserRelationshipController : DbController
	{
		public UserRelationshipController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public List<User> GetRequests(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var requestors = context.UserToUserRelationshipRequests
					.Where(r => r.AcceptorId == id).Select(u => u.Requestor).ToList();

				return requestors;
			}
		}

		public List<User> GetSentRequests(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var acceptors = context.UserToUserRelationshipRequests
					.Where(r => r.RequestorId == id).Select(u => u.Acceptor).ToList();

				return acceptors;
			}
		}

		public List<User> GetFriends(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var requestors = context.UserToUserRelationships
					.Where(r => r.AcceptorId == id)
					.Select(u => u.Requestor).ToList();

				var acceptors = context.UserToUserRelationships
					.Where(r => r.RequestorId == id)
						.Select(u => u.Acceptor).ToList();

				requestors.AddRange(acceptors);

				return requestors;
			}
		}

		// todo move auto accept logic into core controller
		public void Create(UserToUserRelationship newRelation, bool autoAccept)
		{
			using (var context = ContextFactory.Create())
			{
				if (newRelation.AcceptorId == newRelation.RequestorId)
				{
					throw new DuplicateRecordException("Two different users are needed to create a relationship.");
				}

				var hasConflicts = context.UserToUserRelationships
					.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId)
					|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));

				if (!hasConflicts)
				{
					hasConflicts = context.UserToUserRelationshipRequests
					.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId)
					|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));
				}

				if (hasConflicts)
				{
					throw new DuplicateRecordException("A relationship with these users already exists.");
				}

				var requestorExists = context.Users.Any(u => u.Id == newRelation.RequestorId);
				var acceptorExists = context.Users.Any(u => u.Id == newRelation.AcceptorId);

				if (!requestorExists)
				{
					throw new MissingRecordException("The requesting user does not exist.");
				}

				if (!acceptorExists)
				{
					throw new MissingRecordException("The targeted user does not exist.");
				}
				if (autoAccept)
				{
					var relation = new UserToUserRelationship
					{
						RequestorId = newRelation.RequestorId,
						AcceptorId = newRelation.AcceptorId
					};
					context.UserToUserRelationships.Add(relation);
				}
				else
				{
					var relation = new UserToUserRelationshipRequest
					{
						RequestorId = newRelation.RequestorId,
						AcceptorId = newRelation.AcceptorId
					};
					context.UserToUserRelationshipRequests.Add(relation);
				}
				SaveChanges(context);
			}
		}

		// todo move auto accept logic into core controller
		public void UpdateRequest(UserToUserRelationship newRelation, bool accepted)
		{
			using (var context = ContextFactory.Create())
			{
				var relation = context.UserToUserRelationshipRequests
					.Single(r => r.RequestorId == newRelation.RequestorId
					&& r.AcceptorId == newRelation.AcceptorId);

				if (accepted)
				{
					var acceptedRelation = new UserToUserRelationship
					{
						RequestorId = relation.RequestorId,
						AcceptorId = relation.AcceptorId
					};
					context.UserToUserRelationships.Add(acceptedRelation);
				}
				context.UserToUserRelationshipRequests.Remove(relation);
				SaveChanges(context);
			}
		}

		public void Update(UserToUserRelationship newRelation)
		{
			using (var context = ContextFactory.Create())
			{
				var relation = context.UserToUserRelationships
					.Single(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId)
					|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));

				context.UserToUserRelationships.Remove(relation);
				SaveChanges(context);
			}
		}
	}
}
