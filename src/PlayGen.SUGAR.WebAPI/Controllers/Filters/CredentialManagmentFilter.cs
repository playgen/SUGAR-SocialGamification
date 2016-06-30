using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SUGAR.WebAPI.Controllers.Filters
{
    public class CredentialManagmentFilter
    {
		// TODO action filter across all controllers (like ModeValidationFilter) that 
		// intercepts the web request, stores the token from the header (can be used later to re-issue tokens
		// if they timeout etc) and attatches the token to the http response object (IF the response token header value is null)

		// See account controller for token header info
	}
}
