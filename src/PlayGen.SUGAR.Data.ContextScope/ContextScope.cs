using System;
using System.Data.Entity;
using PlayGen.SUGAR.Data.ContextScope.Interfaces;

namespace PlayGen.SUGAR.Data.ContextScope
{
	public class ContextScope : IContextScope
	{
		private static ContextScope _ambientContextScope;
		private IContextFactory _contextFactory;
		private DbContext _context;
		private bool _disposed;

		public ContextScope(IContextFactory contextFactory)
		{
			_contextFactory = contextFactory;
			SetAmbientScope(this);
		}

		public T GetContext<T>() where T : DbContext
		{
			if (_disposed)
				throw new ObjectDisposedException("ContextScope");

			if (_context == null)
			{
				_context = _contextFactory.CreateContext<T>();
			}

			return _context as T;
		}
				
		public int SaveChanges()
		{
			if (_disposed)
				throw new ObjectDisposedException("ContextScope");

			return _context.SaveChanges();
		}

		public void Dispose()
		{
			if (_disposed) return;

			RemoveAmbientScope();
			_context.Dispose();
			_disposed = true;
		}

		#region Ambient
		internal static ContextScope GetAmbientScope()
		{
			return _ambientContextScope;
		}

		private static void SetAmbientScope(ContextScope contextScope)
		{
			if(_ambientContextScope != null)
				throw new Exception("There is already an ambient context scope that should have been disposed before a new one was created.");

			_ambientContextScope = contextScope;
		}

		private static void RemoveAmbientScope()
		{
			_ambientContextScope = null;
		}
		#endregion
	}
}