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
        
        public Game Create(string name)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var hasConflicts = context.Games.Any(g => g.Name == name);

                if (hasConflicts)
                {
                    throw new DuplicateRecordException(string.Format("A game with the name {0} already exists.", name));
                }

                var game = new Game
                {
                    Name = name,
                };
                context.Games.Add(game);
                context.SaveChanges();

                return game;
            }
        }

        public Game Get(string name)
        {
            using (var context = new SGAContext(_nameOrConnectionString))
            {
                SetLog(context);

                var game = context.Games.Single(g => g.Name == name);

                return game;
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
