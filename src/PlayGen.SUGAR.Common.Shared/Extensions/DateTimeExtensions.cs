using System;
using System.Globalization;

namespace PlayGen.SUGAR.Common.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static string SerializeToString(this DateTime dateTime)
        {
            return dateTime.ToString("O", CultureInfo.InvariantCulture);
        }
    }
}
