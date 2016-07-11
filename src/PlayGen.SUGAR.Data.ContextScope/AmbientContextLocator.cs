using System;
using PlayGen.SUGAR.Data.ContextScope.Interfaces;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.ContextScope
{
	public class AmbientContextLocator : IAmbientContextLocator
	{
		public T Get<T>() where T : DbContext
		{
			var contextScope = ContextScope.GetAmbientScope();
			return contextScope == null ? null : contextScope.GetContext<T>();
		}
	}
}
