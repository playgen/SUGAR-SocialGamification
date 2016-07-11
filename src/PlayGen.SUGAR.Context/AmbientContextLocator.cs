using System;
using PlayGen.SUGAR.Data.Context.Interfaces;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.Context
{
	public class AmbientContextLocator : IAmbientContextLocator
	{
		public DbContext Get()
		{
			var contextScope = ContextScope.GetAmbient();
			return contextScope == null ? null : contextScope.Context;
		}
	}
}
