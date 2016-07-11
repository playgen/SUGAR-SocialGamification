using PlayGen.SUGAR.Data.ContextScope.Interfaces;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.ContextScope
{
    public class ContextReadOnlyScope : IContextReadOnlyScope
    {
		private IContextScope _internalScope;
        private bool _disposed = false;

        public ContextReadOnlyScope(IContextFactory contextFactory)
        {
            _internalScope = new ContextScope(contextFactory);
        }

        public T GetContext<T>() where T : DbContext
        {
            return _internalScope.GetContext<T>();
        }
        
		public void Dispose()
		{
            if (_disposed) return;

			_internalScope.Dispose();
            _disposed = false;
		}
	}
}