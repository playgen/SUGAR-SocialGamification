using System;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.Controllers
{
    public abstract class ActorController : Controller
    {
        public int Create(string name)
        {
            throw new NotImplementedException();
        }

        public Actor Get(int id)
        {
            throw new NotImplementedException();
        }

        public Actor Get(string name)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}