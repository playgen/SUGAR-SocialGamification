using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Enums;

namespace PlayGen.SGA.WebAPI.Controllers
{
    public class RelationshipController : Controller
    {
        public void CreateRelationshipRequest(int groupId, int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Actor> GetRelationshipRequests(int groupId)
        {
            throw new NotImplementedException();
        }

        public void UpdateRelationshipRequest(int groupId, int userId, RelationshipStatus status)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Actor> GetRelationshipActors(int groupId)
        {
            throw new NotImplementedException();
        }

        public void UpdateRelationship(int groupId, int userId, RelationshipStatus status)
        {
            throw new NotImplementedException();
        }
    }
}