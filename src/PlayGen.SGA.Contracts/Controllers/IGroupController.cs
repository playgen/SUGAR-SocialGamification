using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGroupController
    {
        ActorResponse Create(ActorRequest actor);

        IEnumerable<ActorResponse> Get(string[] name);

        IEnumerable<ActorResponse> Get();

        void Delete(int[] id);
    }
}
