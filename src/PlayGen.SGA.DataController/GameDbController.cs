using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.DataController
{
    public class GameDbController : DbController
    {
        public GameDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
        
        public Game Create(Game newGame)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.Games.Any(g => g.Name == newGame.Name);

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("A game with the name {0} already exists.", newGame.Name));
                }

                var game = newGame;
                context.Games.Add(game);
                context.SaveChanges();

                return game;
            }
        }

        public IEnumerable<Game> Get(string[] names)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var games = context.Games.Where(g => names.Contains(g.Name));

                return games;
            }
        }

        public void Delete(int id)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var game = context.Games.Single(g => g.Id == id);

                context.Games.Remove(game);
                context.SaveChanges();
            }
        }
    }
}
