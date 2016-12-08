using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                return context.Matches
                    .IncludeAll()
                    .Find(context, matchId);
            }
        }

        public List<Match> GetByTime(DateTime start, DateTime end)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .IncludeAll()
                    .FilterByDateTimeRange(start, end)
                    .ToList();
            }
        }

        public List<Match> GetByGame(int gameId)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .IncludeAll()
                    .Where(m => m.GameId == gameId)
                    .ToList();
            }
        }

        public List<Match> GetByGame(int gameId, DateTime start, DateTime end)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .IncludeAll()
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
                    .IncludeAll()
                    .Where(m => m.CreatorId == creatorId)
                    .ToList();
            }
        }

        public List<Match> GetByCreator(int creatorId, DateTime start, DateTime end)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .IncludeAll()
                    .Where(m => m.CreatorId == creatorId)
                    .FilterByDateTimeRange(start, end)
                    .ToList();
            }
        }

        public List<Match> GetByGameAndCreator(int gameId, int creatorId)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .IncludeAll()
                    .Where(m => m.GameId == gameId && m.CreatorId == creatorId)
                    .ToList();
            }
        }

        public List<Match> GetByGameAndCreator(int gameId, int creatorId, DateTime start, DateTime end)
        {
            using (var context = ContextFactory.Create())
            {
                return context.Matches
                    .IncludeAll()
                    .Where(m => m.GameId == gameId && m.CreatorId == creatorId)
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
                
                return context.Matches
                    .IncludeAll()
                    .Find(context, match.Id);
            }
        }

        public Match Update(Match match)
        {
            using (var context = ContextFactory.Create())
            {
                context.Matches.Update(match);
                SaveChanges(context);

                return context.Matches
                    .IncludeAll()
                    .Find(context, match.Id);
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
