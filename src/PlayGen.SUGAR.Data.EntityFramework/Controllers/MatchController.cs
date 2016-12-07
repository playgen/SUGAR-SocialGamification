using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;
using System.Linq;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
    public class MatchController : DbController
    {
        public MatchController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        public Match Get(int matchId)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches.Find(context, matchId);
            }
        }

        public List<Match> GetByTime(DateTime start, DateTime end)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .FilterByDateTimeRange(start, end)
                    .ToList();
            }
        }

        public List<Match> GetByGame(int gameId)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .Where(m => m.GameId == gameId)
                    .ToList();
            }
        }

        public List<Match> GetByGame(int gameId, DateTime start, DateTime end)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .Where(m => m.GameId == gameId)
                    .FilterByDateTimeRange(start, end) 
                    .ToList();
            }
        }

        public List<Match> GetByCreator(int creatorId)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .Where(m => m.CreatorId == creatorId)
                    .ToList();
            }
        }

        public List<Match> GetByCreator(int creatorId, DateTime start, DateTime end)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .Where(m => m.CreatorId == creatorId)
                    .FilterByDateTimeRange(start, end)
                    .ToList();
            }
        }

        public Match Create(Match match)
        {
            using (var context = ContextFactory.Create())
            {
                context.Matches.Add(match);
                SaveChanges(context);

                return match;
            }
        }

        public void Update(Match match)
        {
            using (var context = ContextFactory.Create())
            {
                context.Matches.Update(match);
                SaveChanges(context);
            }
        }

        public void Delete(int matchId)
        {
            using (var context = ContextFactory.Create())
            {
                var match = context.Matches.Find(context, matchId);
                context.Matches.Remove(match);
                SaveChanges(context);
            }
        }
    }
}
