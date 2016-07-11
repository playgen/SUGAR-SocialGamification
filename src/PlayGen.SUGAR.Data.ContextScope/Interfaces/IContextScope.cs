using System;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.ContextScope.Interfaces
{
    public interface IContextScope : IDisposable
    {
        T GetContext<T>() where T : DbContext;

        int SaveChanges();
    }
}
