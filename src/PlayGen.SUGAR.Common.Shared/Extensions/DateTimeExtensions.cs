using System;
using System.Globalization;

namespace PlayGen.SUGAR.Common.Extensions
{
	public static class DateTimeExtensions
	{
		public static string SerializeToString(this DateTime dateTime)
		{
			return dateTime.ToString("O", CultureInfo.InvariantCulture);
		}
	}
}