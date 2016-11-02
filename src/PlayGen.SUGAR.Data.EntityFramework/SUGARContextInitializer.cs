//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Threading.Tasks;
//using PlayGen.SUGAR.Data.Model;

//namespace PlayGen.SUGAR.Data.EntityFramework
//{
//	// ReSharper disable once InconsistentNaming
//	public class SUGARContextInitializer : DropCreateDatabaseIfModelChanges<SUGARContext>
//	{
//		protected override void Seed(SUGARContext context)
//		{
//			//TODO: add default demo users here

//			context.Accounts.Add(new Account()
//			{
//				Name = "admin",
//				Password = "$2a$12$SSIgQE0cQejeH0dM61JV/eScAiHwJo/I3Gg6xZFUc0gmwh0FnMFv.",
//				Id = 1,
//				User = new User()
//				{
//					Id = 1,
//					Name = "admin"
//				},
//				UserId = 1
//			});

//		}
//	}
//}
