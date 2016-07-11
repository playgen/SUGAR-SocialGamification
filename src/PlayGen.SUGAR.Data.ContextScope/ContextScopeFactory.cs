using System;
using PlayGen.SUGAR.Data.ContextScope.Interfaces;

namespace PlayGen.SUGAR.Data.ContextScope
{
	public class ContextScopeFactory : IContextScopeFactory
	{
		private readonly IContextFactory _contextFactory;

		public ContextScopeFactory(string connectionString)
		{
			_contextFactory = new ContextFactory(connectionString);
		}

		public IContextScope Create()
		{
			return new ContextScope(_contextFactory);
		}

		public IContextReadOnlyScope CreateReadOnly()
		{
			return new ContextReadOnlyScope(_contextFactory);
		}
	}
}
