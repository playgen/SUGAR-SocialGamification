namespace PlayGen.SUGAR.Client
{
	public interface IHttpHandler
	{
		HttpResponse HandleRequest(HttpRequest request);
	}
}