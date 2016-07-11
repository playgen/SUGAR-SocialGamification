using System.Data.Entity;

namespace PlayGen.SUGAR.Data.Context.Interfaces
{
	public interface IAmbientContextLocator
	{
		DbContext Get();
	}
}
