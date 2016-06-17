using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGameController
    {
        GameResponse Create(GameRequest game);

        IEnumerable<GameResponse> Get(string[] name);

        IEnumerable<GameResponse> Get();

        void Delete(int[] id);
    }
}