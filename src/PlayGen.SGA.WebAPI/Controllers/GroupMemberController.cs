using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Enums;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GroupMemberController : RelationshipController
    {
    }
}