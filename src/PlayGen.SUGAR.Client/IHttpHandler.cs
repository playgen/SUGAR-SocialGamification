using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PlayGen.SUGAR.Client
{
    public interface IHttpHandler
    {
	    HttpResponse HandleRequest(HttpRequest request);

		HttpResponse GetDelete(HttpRequest request);

		HttpResponse PostPut(HttpRequest request);
	}
}
