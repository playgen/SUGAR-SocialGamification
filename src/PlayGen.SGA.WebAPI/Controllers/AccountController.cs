using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller, IAccountController
    {
        // POST api/account
        [HttpPost]
        public void Register(string name, string password)
        {
            throw new NotImplementedException();
        }

        // GET api/account?name=bob&password=...
        [HttpGet]
        public string Login(string name, string password)
        {
            throw new NotImplementedException();
        }

        // DELETE api/game?accountId=1&accountId=2
        [HttpDelete]
        public void Delete(string[] id)
        {
            throw new NotImplementedException();
        }
    }
}
