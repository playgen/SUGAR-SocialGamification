using System;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.Context.Interfaces
{
    public interface IContextScope : IDisposable
    {
        DbContext Context { get; }

        int SaveChanges();
    }
}
