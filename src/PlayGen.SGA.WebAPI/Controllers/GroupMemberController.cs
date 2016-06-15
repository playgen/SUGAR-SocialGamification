using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GroupMemberController : Controller, IGroupMemberController
    {
        // POST api/groupmember/1/request/2
        [HttpPost("{userId}/request/{groupId}")]
        public void CreateMemberRequest(int userId, int groupId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupmember/1/requests
        [HttpGet("{groupId}/requests")]
        public IEnumerable<Actor> GetMemberRequests(int groupId)
        {
            throw new NotImplementedException();
        }

        // PUT api/groupmember/1/request
        [HttpPut("{userId}/request/")]
        public void UpdateMemberRequest(int userId, [FromBody] Relationship relationship)
        {
            throw new NotImplementedException();
        }

        // GET api/groupmember/1/members
        [HttpGet("{groupId}/members")]
        public IEnumerable<Actor> GetMembers(int groupId)
        {
            throw new NotImplementedException();
        }

        // PUT api/groupmember/1/member
        [HttpPut("{groupId}/member")]
        public void UpdateMember(int userId, Relationship relationship)
        {
            throw new NotImplementedException();
        }
    }
}