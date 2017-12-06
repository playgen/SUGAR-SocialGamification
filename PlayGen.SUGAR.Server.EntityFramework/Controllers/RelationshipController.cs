using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class RelationshipController : DbController
	{
		public RelationshipController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public List<Actor> GetRequests(int id, ActorType fromActorType)
		{
			using (var context = ContextFactory.Create())
			{
				var requestors = context.RelationshipRequests
					.Where(r => r.AcceptorId == id && r.Requestor.ActorType == fromActorType)
					.Select(u => u.Requestor).ToList();

				return requestors;
			}
		}

		public List<Actor> GetSentRequests(int id, ActorType toActorType)
		{
			using (var context = ContextFactory.Create())
			{
				var acceptors = context.RelationshipRequests
					.Where(r => r.RequestorId == id && r.Acceptor.ActorType == toActorType)
					.Select(u => u.Acceptor).ToList();

				return acceptors;
			}
		}

		public List<Actor> GetRelationships(int id, ActorType relationshipActorType)
		{
			using (var context = ContextFactory.Create())
			{
				var requestors = context.Relationships
					.Where(r => r.AcceptorId == id && r.Requestor.ActorType == relationshipActorType)
					.Select(u => u.Requestor).ToList();

				var acceptors = context.Relationships
					.Where(r => r.RequestorId == id && r.Acceptor.ActorType == relationshipActorType)
					.Select(u => u.Acceptor).ToList();

				requestors.AddRange(acceptors);

				return requestors;
			}
		}

		public int GetRelationshipCount(int id, ActorType relationshipActorType)
		{
			return GetRelationships(id, relationshipActorType).Count;
		}

		/// <summary>
		/// Immediately creates a new relationship between 2 actors
		/// </summary>
		/// <param name="newRelation">Relationship to create</param>
		public void CreateRelationship(ActorRelationship newRelation)
		{
			using (var context = ContextFactory.Create())
			{
				if (newRelation.AcceptorId == newRelation.RequestorId)
				{
					throw new DuplicateRecordException("Two different users are needed to create a relationship.");
				}

				var hasConflicts = context.Relationships
					.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId)
							|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));

				if (!hasConflicts)
				{
					hasConflicts = context.RelationshipRequests
						.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId)
								|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));
				}

				if (hasConflicts)
				{
					throw new DuplicateRecordException("A relationship with these users already exists.");
				}

				var requestorExists = context.Users.Any(u => u.Id == newRelation.RequestorId);
				if (!requestorExists)
				{
					throw new MissingRecordException("The requesting user does not exist.");
				}

				var acceptorExists = context.Users.Any(u => u.Id == newRelation.AcceptorId);
				if (!acceptorExists)
				{
					throw new MissingRecordException("The targeted user does not exist.");
				}
				var relation = new ActorRelationship
				{
					RequestorId = newRelation.RequestorId,
					AcceptorId = newRelation.AcceptorId
				};
				context.Relationships.Add(relation);
				SaveChanges(context);
			}
		}

		/// <summary>
		/// Create a new relationship request between 2 actors
		/// </summary>
		/// <param name="newRelation">Relationship Request to create</param>
		public void CreateRelationshipRequest(ActorRelationship newRelation)
		{
			using (var context = ContextFactory.Create())
			{
				if (newRelation.AcceptorId == newRelation.RequestorId)
				{
					throw new DuplicateRecordException("Two different users are needed to create a relationship.");
				}

				var hasConflicts = context.Relationships
					.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId)
							|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));

				if (!hasConflicts)
				{
					hasConflicts = context.RelationshipRequests
						.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId)
								|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));
				}

				if (hasConflicts)
				{
					throw new DuplicateRecordException("A relationship with these users already exists.");
				}

				var requestorExists = context.Users.Any(u => u.Id == newRelation.RequestorId);
				if (!requestorExists)
				{
					throw new MissingRecordException("The requesting user does not exist.");
				}

				var acceptorExists = context.Users.Any(u => u.Id == newRelation.AcceptorId);
				if (!acceptorExists)
				{
					throw new MissingRecordException("The targeted user does not exist.");
				}

				var relation = new ActorRelationshipRequest
				{
					RequestorId = newRelation.RequestorId,
					AcceptorId = newRelation.AcceptorId
				};
				context.RelationshipRequests.Add(relation);
				SaveChanges(context);
			}
		}

		public void UpdateRequest(ActorRelationship newRelation, bool accepted)
		{
			using (var context = ContextFactory.Create())
			{
				var relation = context.RelationshipRequests
					.Single(r => r.RequestorId == newRelation.RequestorId
								&& r.AcceptorId == newRelation.AcceptorId);

				if (accepted)
				{
					var acceptedRelation = new ActorRelationship
					{
						RequestorId = relation.RequestorId,
						AcceptorId = relation.AcceptorId
					};
					context.Relationships.Add(acceptedRelation);
				}
				context.RelationshipRequests.Remove(relation);
				SaveChanges(context);
			}
		}

		public void Update(ActorRelationship newRelation)
		{
			using (var context = ContextFactory.Create())
			{
				var relation = context.Relationships
					.Single(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId)
								|| (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));

				context.Relationships.Remove(relation);
				SaveChanges(context);
			}
		}
	}
}
