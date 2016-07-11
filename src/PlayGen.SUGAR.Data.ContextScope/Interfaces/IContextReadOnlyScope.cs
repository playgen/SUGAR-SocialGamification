using System;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.ContextScope.Interfaces
{
    public interface IContextReadOnlyScope : IDisposable
    {
        T GetContext<T>() where T : DbContext;
    }
}
