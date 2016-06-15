using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IUserController
    {
        int Create(Actor actor);

        IEnumerable<Actor> Get(string[] name);

        void Delete(int id);
    }
}
