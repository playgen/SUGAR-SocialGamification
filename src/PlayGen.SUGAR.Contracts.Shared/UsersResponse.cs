using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class UsersResponse : Response
	{
		public UserResponse[] Items { get; set; }
	}
}
