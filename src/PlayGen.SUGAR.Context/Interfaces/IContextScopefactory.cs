namespace PlayGen.SUGAR.Data.Context.Interfaces
{
    /// <summary>
    /// Convenience methods to create a new ambient DbContextScope.
    /// </summary>
    public interface IContextScopeFactory
    {
        IContextScope Create();

        IContextReadOnlyScope CreateReadOnly();
    }
}