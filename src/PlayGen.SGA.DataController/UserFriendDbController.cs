using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.DataController
{
    public class UserFriendDbController : DbController
    {
        public UserFriendDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public void Create(UserToUserRelationship newRelation)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.UserToUserRelationships.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId) || (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));
                if (!hasConflicts) {
                    hasConflicts = context.UserToUserRelationshipRequests.Any(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId) || (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));
                }

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("A relationship with these users already exists."));
                }

                var requestorExists = context.Users.Any(u => u.Id == newRelation.RequestorId);
                var acceptorExists = context.Users.Any(u => u.Id == newRelation.AcceptorId);

                if (!requestorExists)
                {
                    throw new DuplicateRecordException(string.Format("The requesting user does not exist."));
                }

                if (!acceptorExists)
                {
                    throw new DuplicateRecordException(string.Format("The targeted user does not exist."));
                }

                var relation = new UserToUserRelationshipRequest
                {
                    RequestorId = newRelation.RequestorId,
                    AcceptorId = newRelation.AcceptorId
                };
                context.UserToUserRelationshipRequests.Add(relation);
                context.SaveChanges();
            }
        }

        public IEnumerable<User> GetRequests(int id)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var requestors = context.UserToUserRelationshipRequests.Where(r => r.AcceptorId == id).Select(u => u.Requestor).ToList();

                return requestors;
            }
        }

        public void UpdateRequest(UserToUserRelationship newRelation, bool accepted)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var relation = context.UserToUserRelationshipRequests.Single(r => r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId);

                if (accepted) {
                    var acceptedRelation = new UserToUserRelationship
                    {
                        RequestorId = relation.RequestorId,
                        AcceptorId = relation.AcceptorId
                    };
                    context.UserToUserRelationships.Add(acceptedRelation);
                }
                context.UserToUserRelationshipRequests.Remove(relation);
                context.SaveChanges();
            }
        }

        public IEnumerable<User> GetFriends(int id)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var requestors = context.UserToUserRelationships.Where(r => r.AcceptorId == id).Select(u => u.Requestor).ToList();
                var acceptors = context.UserToUserRelationships.Where(r => r.RequestorId == id).Select(u => u.Acceptor).ToList();
                requestors.AddRange(acceptors);

                return requestors;
            }
        }

        public void Update(UserToUserRelationship newRelation)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var relation = context.UserToUserRelationships.Single(r => (r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId) || (r.RequestorId == newRelation.AcceptorId && r.AcceptorId == newRelation.RequestorId));

                context.UserToUserRelationships.Remove(relation);
                context.SaveChanges();
            }
        }
    }
}
