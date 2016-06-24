using System.Data.Entity;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class UserDataController : GameDataControllerBase<UserData>, IGameDataController
	{
		public UserDataController(string nameOrConnectionString) 
			: base(nameOrConnectionString)
		{
		}

		public UserData Create(UserData data)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var actor = context.Users.SingleOrDefault(u => u.Id == data.UserId);
				var game = context.Games.SingleOrDefault(g => g.Id == data.GameId);

				if (actor == null)
				{
					throw new MissingRecordException(string.Format("The provided user does not exist."));
				}

				if (game == null)
				{
					throw new MissingRecordException(string.Format("The provided game does not exist."));
				}

				if (data.User == null)
				{
					data.User = actor;
				}
				else if (context.Entry(data.User).State == EntityState.Detached)
				{
					context.Users.Attach(data.User);
				}

				if (data.Game == null)
				{
					data.Game = game;
				}			   
				else if (context.Entry(data.Game).State == EntityState.Detached)
				{
					context.Games.Attach(data.Game);
				}

				context.UserData.Add(data);
				SaveChanges(context);
				return data;
			}
		}
	}
}
