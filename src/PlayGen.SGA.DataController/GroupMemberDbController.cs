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
    public class GroupMemberDbController : DbController
    {
        public GroupMemberDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public void Create(UserToGroupRelationship newRelation)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.UserToGroupRelationships.Any(r => r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId);
                if (!hasConflicts)
                {
                    hasConflicts = context.UserToGroupRelationshipRequests.Any(r => r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId);
                }

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("A relationship with this user and group already exists."));
                }

                var requestorExists = context.Users.Any(u => u.Id == newRelation.RequestorId);
                var acceptorExists = context.Groups.Any(g => g.Id == newRelation.AcceptorId);

                if (!requestorExists) {
                    throw new DuplicateRecordException(string.Format("The requesting user does not exist."));
                }

                if (!acceptorExists) {
                    throw new DuplicateRecordException(string.Format("The targeted group does not exist."));
                }

                var relation = new UserToGroupRelationshipRequest
                {
                    RequestorId = newRelation.RequestorId,
                    AcceptorId = newRelation.AcceptorId
                };
                context.UserToGroupRelationshipRequests.Add(relation);
                context.SaveChanges();
                UpdateRequest(newRelation, true);
            }
        }

        public IEnumerable<User> GetRequests(int id)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var requestors = context.UserToGroupRelationshipRequests.Where(r => r.AcceptorId == id).Select(u => u.Requestor).ToList();

                return requestors;
            }
        }

        public void UpdateRequest(UserToGroupRelationship newRelation, bool accepted)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
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
                context.SaveChanges();
            }
        }

        public IEnumerable<User> GetMembers(int id)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var requestors = context.UserToGroupRelationships.Where(r => r.AcceptorId == id).Select(u => u.Requestor).ToList();

                return requestors;
            }
        }

        public void Update(UserToGroupRelationship newRelation)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var relation = context.UserToGroupRelationships.Single(r => r.RequestorId == newRelation.RequestorId && r.AcceptorId == newRelation.AcceptorId);

                context.UserToGroupRelationships.Remove(relation);
                context.SaveChanges();
            }
        }
    }
}