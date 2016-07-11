using System;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.Context.Interfaces
{
    public interface IContextReadOnlyScope : IDisposable
    {
        DbContext Context { get; }
    }
}
