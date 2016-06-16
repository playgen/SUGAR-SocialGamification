using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    public class GameClientProxy : ClientProxy, IGameController
    {
        public int Create(Game game)
        {
            return base.Post<Game, int>("api/game", game);
        }

        public IEnumerable<Game> Get(string[] name)
        {
            var builder = new StringBuilder();
            builder.Append("api/game?");
            if (name.Length > 0)
            {
                foreach (var nm in name)
                {
                    builder.AppendFormat("name={0}&", nm);
                }
                builder.Remove(builder.Length - 1, 1);
            }
            return base.Get<IEnumerable<Game>>(builder.ToString());
        }

        public void Delete(int id)
        {
            base.Delete("api/game/" + id);
        }
    }
}
