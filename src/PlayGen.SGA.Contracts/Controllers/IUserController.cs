using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IUserController
    {
        IEnumerable<ActorResponse> Get();

        IEnumerable<ActorResponse> Get(string[] name);

        ActorResponse Create(ActorRequest actor);

        void Delete(int[] id);
    }
}
