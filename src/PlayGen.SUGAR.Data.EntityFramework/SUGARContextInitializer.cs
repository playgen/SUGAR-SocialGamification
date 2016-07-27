using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework
{
	// ReSharper disable once InconsistentNaming
	public class SUGARContextInitializer : DropCreateDatabaseIfModelChanges<SUGARContext>
	{
		protected override void Seed(SUGARContext context)
		{
			//TODO: add default demo users here

			//context.Users.Add(new User()
			//{
				
			//})

		}
	}
}
