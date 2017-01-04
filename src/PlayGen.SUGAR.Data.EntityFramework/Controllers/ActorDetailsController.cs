using System.Collections.Generic;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using System.Linq;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class ActorDetailsController : DbController
	{
		public ActorDetailsController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public bool KeyExists(int actorId, string key)
		{
			using (var context = ContextFactory.Create())
			{
				return context.ActorDetails.FilterByActorId(actorId)
					.FilterByKey(key)
					.Any();
			}
		}

		public List<ActorDetails> Get(int actorId, ICollection<string> keys = null)
		{
			using (var context = ContextFactory.Create())
			{
				var data = context.ActorDetails.FilterByActorId(actorId)
					.FilterByKeys(keys)
					.ToList();
				return data;
			}
		}

		public ActorDetails Create(ActorDetails details)
		{
			using (var context = ContextFactory.Create())
			{
				context.HandleDetatchedActor(details.ActorId);

				context.ActorDetails.Add(details);
				SaveChanges(context);

				return details;
			}
		}

		public void Update(ActorDetails updatedDetails)
		{
			using (var context = ContextFactory.Create())
			{
				// todo replace with entire block with: (and update unit tests)
				// context.[tablename].Update(entity);
				// context.SaveChanges();

				var existingData = context.ActorDetails
					.Find(context, updatedDetails.Id);

				if (existingData == null)
				{
					throw new MissingRecordException("Cannot find the object to update.");
				}

				existingData.Value = updatedDetails.Value;

				SaveChanges(context);
			}
		}
	}
}