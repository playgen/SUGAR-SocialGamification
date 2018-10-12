using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;
using PlayGen.SUGAR.Server.Model.Interfaces;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class RelationshipController : DbController
	{
		public RelationshipController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public List<Actor> GetRelationRequestorActors(int id, ActorType fromActorType)
		{
			using (var context = ContextFactory.Create())
			{
				var requestors = context.RelationshipRequests
					.Where(r => r.AcceptorId == id && r.Requestor.ActorType == fromActorType)
					.Select(u => u.Requestor).ToList();

				return requestors;
			}
		}

		public List<Actor> GetRelationAcceptorActors(int id, ActorType toActorType)
		{
			using (var context = ContextFactory.Create())
			{
				var acceptors = context.RelationshipRequests
					.Where(r => r.RequestorId == id && r.Acceptor.ActorType == toActorType)
					.Select(u => u.Acceptor).ToList();

				return acceptors;
			}
		}

		public List<Actor> GetRelatedActors(int id, ActorType relationshipActorType)
		{
			using (var context = ContextFactory.Create())
			{
				var relationships = context.Relationships;

				var requestors = relationships
					.Where(r => r.AcceptorId == id && r.Requestor.ActorType == relationshipActorType)
					.Select(u => u.Requestor).ToList();

				var acceptors = relationships
					.Where(r => r.RequestorId == id && r.Acceptor.ActorType == relationshipActorType)
					.Select(u => u.Acceptor).ToList();			

				return requestors.Concat(acceptors).ToList();
			}
		}

		public ActorRelationship GetRelationship(int relatedActorA, int relatedActorB)
		{
			using (var context = ContextFactory.Create())
			{
				var relationship = context.Relationships
					.Single(r => AreRelated(r, relatedActorA, relatedActorB));
				return relationship;
			}
		}

		public List<ActorRelationship> GetRelationships(int actorId, ActorType relationshipActorType)
		{
			using (var context = ContextFactory.Create())
			{
				var relationships = context.Relationships
					.Where(r => (r.AcceptorId == actorId && r.Requestor.ActorType == relationshipActorType)
								|| (r.RequestorId == actorId && r.Acceptor.ActorType == relationshipActorType))
					.ToList();

				return relationships;
			}
        }

        public int GetRelationshipCount(int id, ActorType relationshipActorType)
		{
			return GetRelatedActors(id, relationshipActorType).Count;
		}

        /// <summary>
        /// Immediately creates a new relationship between 2 actors
        /// </summary>
        /// <param name="newRelation">Relationship to create</param>
        /// <param name="context">Optional DbContext to perform opperations on. If ommitted a DbContext will be created.</param>
        public void CreateRelationship(ActorRelationship newRelation, SUGARContext context = null)
		{
			var didCreateContext = false;
			if (context == null)
			{
				context = ContextFactory.Create();
				didCreateContext = true;
			}

			if (newRelation.AcceptorId == newRelation.RequestorId)
			{
				throw new InvalidRelationshipException("Two different users are needed to create a relationship.");
			}

            var hasConflicts = context.Relationships
				.Any(r => AreRelated(r, newRelation));

            if (!hasConflicts)
			{
				hasConflicts = context.RelationshipRequests
					.Any(r => AreRelated(r, newRelation));
            }

			if (hasConflicts)
			{
				throw new DuplicateRelationshipException("A relationship with these users already exists.");
			}

			var requestorExists = context.Actors.Any(u => u.Id == newRelation.RequestorId);
			if (!requestorExists)
			{
				throw new InvalidRelationshipException("The requesting user does not exist.");
			}

			var acceptorExists = context.Actors.Any(u => u.Id == newRelation.AcceptorId);
			if (!acceptorExists)
			{
				throw new InvalidRelationshipException("The targeted user does not exist.");
			}

			var relation = new ActorRelationship
			{
				RequestorId = newRelation.RequestorId,
				AcceptorId = newRelation.AcceptorId
			};

			context.Relationships.Add(relation);

            if (didCreateContext)
            { 
				context.SaveChanges();
				context.Dispose();
			}
		}

		/// <summary>
		/// Create a new relationship request between 2 actors
		/// </summary>
		/// <param name="newRelation">Relationship Request to create</param>
		public void CreateRelationshipRequest(ActorRelationship newRelation, SUGARContext context = null)
		{
			var didCreateContext = false;
			if (context == null)
			{
				context = ContextFactory.Create();
				didCreateContext = true;
			}
			
			if (newRelation.AcceptorId == newRelation.RequestorId)
			{
				throw new DuplicateRecordException("Two different users are needed to create a relationship.");
			}

			var hasConflicts = context.Relationships
				.Any(r => AreRelated(r, newRelation));

            if (!hasConflicts)
			{
				hasConflicts = context.RelationshipRequests
					.Any(r => AreRelated(r, newRelation));
			}

			if (hasConflicts)
			{
				throw new DuplicateRecordException("A relationship with these users already exists.");
			}

			var requestorExists = context.Actors.Any(u => u.Id == newRelation.RequestorId);
			if (!requestorExists)
			{
				throw new MissingRecordException("The requesting user does not exist.");
			}

			var acceptorExists = context.Actors.Any(u => u.Id == newRelation.AcceptorId);
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

			if(didCreateContext)
            { 
				context.SaveChanges();
				context.Dispose();
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
				context.SaveChanges();
			}
		}

		public void Delete(ActorRelationship newRelation)
		{
			using (var context = ContextFactory.Create())
			{
				var relation = context.Relationships
					.Single(r => AreRelated(r, newRelation));

				context.Relationships.Remove(relation);
				context.SaveChanges();
			}
		}

		private bool AreRelated(IRelationship relationshipA, IRelationship relationshipB)
		{
			return AreRelated(relationshipA, relationshipB.AcceptorId, relationshipB.RequestorId);
		}

        private bool AreRelated(IRelationship relationship, int relatedActorA, int relatedActorB)
		{
			return relationship.AcceptorId == relatedActorA && relationship.RequestorId == relatedActorB
					|| relationship.AcceptorId == relatedActorB && relationship.RequestorId == relatedActorA;
        }
	}
}
