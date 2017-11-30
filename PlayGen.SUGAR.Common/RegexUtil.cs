using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PlayGen.SUGAR.Common
{
    public static class RegexUtil
	{
		public const string AlphaNumericUnderscoreNotEmpty = @"^\w+$";

		public static Regex AlphaNumericUnderscoreNotEmptyRegex = new Regex(AlphaNumericUnderscoreNotEmpty);

		public static bool IsAlphaNumericUnderscoreNotEmpty(string check)
		{
			return AlphaNumericUnderscoreNotEmptyRegex.IsMatch(check);
		}
	}
}
