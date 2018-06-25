namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public abstract class DbController
	{
		protected readonly SUGARContextFactory ContextFactory;

		protected DbController(SUGARContextFactory contextFactory)
		{
			ContextFactory = contextFactory;
		}
	}
}
