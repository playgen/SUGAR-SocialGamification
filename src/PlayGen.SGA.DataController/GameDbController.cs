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

        /// <summary>
        /// Retrieve game multiple records by name from the database
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public IEnumerable<Game> Get()
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var games = context.Games.ToList();

                return games;
            }
        }

        public IEnumerable<Game> Get(string[] names)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var games = context.Games.Where(g => names.Contains(g.Name)).ToList();

                return games;
            }
        }

        /// <summary>
        /// Create a new game record in the database.
        /// </summary>
        /// <param name="newGame"></param>
        /// <returns></returns>
        public Game Create(Game newGame)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                /*
                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("A game with the name {0} already exists.", newGame.Name));
                }*/

                var game = newGame;
                context.Games.Add(game);
                context.SaveChanges();

                return game;
            }
        }

        /// <summary>
        /// Delete a game record from the database.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int[] id)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var games = context.Games.Where(g => id.Contains(g.Id)).ToList();

                context.Games.RemoveRange(games);
                context.SaveChanges();
            }
        }
    }
}
