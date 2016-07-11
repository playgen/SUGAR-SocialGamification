using System.Data.Entity;

namespace PlayGen.SUGAR.Data.ContextScope.Interfaces
{
	public interface IAmbientContextLocator
	{
		T Get<T>() where T : DbContext;
	}
}
