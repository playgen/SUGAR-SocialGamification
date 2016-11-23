namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    internal static class EvaluationEventExtensions
    {
        internal static int ToInt(this int? nullableInt)
        {
            return nullableInt ?? 0;
        }
    }
}
