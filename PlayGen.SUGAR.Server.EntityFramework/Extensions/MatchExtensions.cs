using System;
using System.Linq;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Extensions
{
	public static class MatchExtensions
	{
		public static IQueryable<Match> FilterByDateTimeRange(this IQueryable<Match> matches, DateTime? start, DateTime? end)
		{
			if (start == null && end == null)
			{
				return matches.Where(m => m.Started == null && m.Ended == null);
			}
			return end == null ? matches.Where(m => m.Started != null && start <= m.Started && m.Ended == null) : matches.Where(m => m.Started != null && start <= m.Started && m.Ended != null && m.Ended <= end);
		}
	}
}
