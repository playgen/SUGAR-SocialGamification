using PlayGen.SUGAR.Data.Context.Interfaces;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.Context
{
    public class ContextReadOnlyScope : IContextReadOnlyScope
    {
		private ContextScope _internalScope;

		public DbContext Context => _internalScope.Context;

		public ContextReadOnlyScope(IContextFactory contextFactory)
		{
			_internalScope = new ContextScope(contextFactory);
		}

		public void Dispose()
		{
			_internalScope.Dispose();
		}
	}
}