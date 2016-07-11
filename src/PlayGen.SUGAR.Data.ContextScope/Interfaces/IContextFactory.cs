using System.Data.Entity;

namespace PlayGen.SUGAR.Data.ContextScope.Interfaces
{
    public interface IContextFactory
    {
        T CreateContext<T>() where T : DbContext;
    }
}
