using System;
using System.Data.Entity;
using PlayGen.SUGAR.Data.Context.Interfaces;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;

namespace PlayGen.SUGAR.Data.Context
{
	public class ContextScope : IContextScope
	{
		private static ContextScope _ambientContextScope;
		private DbContext _context;

		public DbContext Context => _context;

		public ContextScope(IContextFactory contextFactory)
		{
			_context = contextFactory.CreateContext();
			SetAmbient(this);
		}

		public void Dispose()
		{
			RemoveAmbient();
			_context.Dispose();			
		}

		public int SaveChanges()
		{
			return _context.SaveChanges();
		}
				
		internal static ContextScope GetAmbient()
		{
			return _ambientContextScope;
		}

		private static void SetAmbient(ContextScope contextScope)
		{
			if(_ambientContextScope != null)
			{
				throw new Exception("There is already an ambient context scope that should have been disposed before a new one was created.");
			}

			_ambientContextScope = contextScope;
		}

		private static void RemoveAmbient()
		{
			_ambientContextScope = null;
		}
	}
}