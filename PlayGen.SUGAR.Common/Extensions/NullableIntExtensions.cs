namespace PlayGen.SUGAR.Common.Shared.Extensions
{
    public static class NullableIntExtensions
    {
        public static int ToInt(this int? nullableInt)
        {
            return nullableInt ?? 0;
        }
    }
}
