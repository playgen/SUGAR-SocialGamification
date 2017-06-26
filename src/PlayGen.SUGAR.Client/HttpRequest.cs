using System.Collections.Generic;

namespace PlayGen.SUGAR.Client
{
	public class HttpRequest
	{
		public string Url { get; set; }

		public string Method { get; set; }

		public string Content { get; set; }

		public Dictionary<string, string> Headers { get; set; }
	}
}