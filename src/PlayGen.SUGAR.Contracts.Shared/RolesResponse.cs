using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class RolesResponse : Response
	{
		public RoleResponse[] Items { get; set; }
	}
}
