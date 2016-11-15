namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class ActorDataController : DbController
	{
		public ActorDataController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }
    }
}
