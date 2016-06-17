using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    public class GroupClientProxy : ClientProxy, IGroupController
    {
        public int Create(Actor actor)
        {
            return base.Post<Actor, int>("api/game", actor);
        }

        public IEnumerable<Actor> Get(string[] name)
        {
            throw new NotImplementedException();
        }

        public void Delete(int[] id)
        {
            throw new NotImplementedException();
        }
    }
}
