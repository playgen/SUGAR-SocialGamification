using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    public class GameClientProxy : ClientProxy, IGameController
    {
        public IEnumerable<GameResponse> Get()
        {
            var query = GetUriBuilder("api/game/all").ToString();
            return Get<IEnumerable<GameResponse>>(query);
        }

        public IEnumerable<GameResponse> Get(string[] name)
        {
            var query = GetUriBuilder("api/game")
                .AppendQueryParameters(name, "name={0}")
                .ToString();
            return Get<IEnumerable<GameResponse>>(query);
        }

        public GameResponse Create(GameRequest game)
        {
            var query = GetUriBuilder("api/game").ToString();
            throw new NotImplementedException();
            //return Post<GameResponse, int>(query, game);
        }

        public void Delete(int[] id)
        {
            var query = GetUriBuilder("api/game")
                .AppendQueryParameters(id, "id={0}")
                .ToString();
            Delete(query);
        }
    }
}
