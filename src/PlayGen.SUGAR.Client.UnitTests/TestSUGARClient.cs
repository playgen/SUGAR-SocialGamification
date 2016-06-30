namespace PlayGen.SUGAR.Client.UnitTests
{
    public class TestSUGARClient : SUGARClient
    {
	    private const string BaseAddress = "http://localhost:62312/";

		public TestSUGARClient() : base(BaseAddress)
		{
	    }
	}
}
