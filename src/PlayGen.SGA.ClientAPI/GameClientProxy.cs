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
        public int Create(Game game)
        {
            var query = GetUriBuilder("api/game").ToString();
            return Post<Game, int>(query, game);
        }

        public IEnumerable<Game> Get(string[] name)
        {
            var query = GetUriBuilder("api/game")
                .AppendQueryParameters(name, "name={0}")
                .ToString();
            return Get<IEnumerable<Game>>(query);
        }

        public IEnumerable<Game> Get()
        {
            var query = GetUriBuilder("api/game/all").ToString();
            return Get<IEnumerable<Game>>(query);
        }


        public void Delete(int id)
        {
            var query = GetUriBuilder("api/game/" + id).ToString();
            Delete(query);
        }

        public void Delete(int[] id)
        {
            var query = GetUriBuilder("api/game")
                .AppendQueryParameters(id, "id={0}")
                .ToString();
            Delete(query);
        }

        public new IEnumerable<Game> Get()
        {
            throw new NotImplementedException();
        }
    }
}
