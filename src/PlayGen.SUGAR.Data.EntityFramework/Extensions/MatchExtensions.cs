using System;
using System.Linq;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
    public static class MatchExtensions
    {
        public static IQueryable<Match> FilterByDateTimeRange(this IQueryable<Match> matches, DateTime start, DateTime end)
        {
            return matches.Where(m => m.Started != default(DateTime) && m.Ended != null
                                      && start <= m.Started && m.Ended <= end);
        }
    }
}
