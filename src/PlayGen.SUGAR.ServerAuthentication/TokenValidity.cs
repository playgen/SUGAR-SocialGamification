using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SUGAR.ServerAuthentication
{
	public enum TokenValidity
	{
		Invalid = 0,
		Valid,
		Expired,
	}
}
