using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGroupController
    {
        int Create(Actor actor);

        IEnumerable<Actor> Get(string[] name);

        IEnumerable<Actor> Get();

        void Delete(int[] id);
    }
}
