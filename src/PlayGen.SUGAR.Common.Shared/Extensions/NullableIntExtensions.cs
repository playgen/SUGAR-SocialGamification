namespace PlayGen.SUGAR.Common.Extensions
{
	public static class NullableIntExtensions
	{
		public static int ToInt(this int? nullableInt)
		{
			return nullableInt ?? 0;
		}
	}
}