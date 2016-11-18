using System.Collections.Generic;

namespace PlayGen.SUGAR.Client
{
    public class HttpResponse
    {
		public string Content { get; set; }

	    public Dictionary<string, string> Headers { get; set; }

		public int StatusCode { get; set; }
    }
}
