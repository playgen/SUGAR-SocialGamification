using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GroupDataController : GameDataControllerBase<GroupData>, IGameDataController
	{
		public GroupDataController(string nameOrConnectionString) 
			: base(nameOrConnectionString)
		{
		}

		public GroupData Create(GroupData data)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var group = context.Groups.SingleOrDefault(g => g.Id == data.GroupId);
				if (group == null)
				{
					throw new MissingRecordException("The specified group does not exist.");
				}

				var game = context.Games.SingleOrDefault(g => g.Id == data.GameId);
				if (game == null)
				{
					throw new MissingRecordException("The specified game does not exist.");
				}

				if (data.Group == null)
				{
					data.Group = group;
				}
				//TODO: we shouldnt need to be testing attached states
				else if (context.Entry(data.Group).State == EntityState.Detached)
				{
					context.Groups.Attach(data.Group);
				}

				if (data.Game == null)
				{
					data.Game = game;
				}			   
				else if (context.Entry(data.Game).State == EntityState.Detached)
				{
					context.Games.Attach(data.Game);
				}

				context.GroupData.Add(data);
				SaveChanges(context);
				return data;
			}
		}
	}
}
