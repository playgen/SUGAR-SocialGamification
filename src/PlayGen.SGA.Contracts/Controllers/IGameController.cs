using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGameController
    {
        int Create(Game game);

        IEnumerable<Game> Get(string[] name);

        IEnumerable<Game> Get();

        void Delete(int[] id);
    }
}