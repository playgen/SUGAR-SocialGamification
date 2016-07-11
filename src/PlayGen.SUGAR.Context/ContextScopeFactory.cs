using System;
using PlayGen.SUGAR.Data.Context.Interfaces;

namespace PlayGen.SUGAR.Data.Context
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
