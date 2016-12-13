using System;
using System.Globalization;

namespace PlayGen.SUGAR.Common.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToSUGARString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        }
    }
}
