using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	[Collection(nameof(ClientTestsFixture))]
	public class ClientTestBase
	{
		protected readonly ClientTestsFixture Fixture;

		public ClientTestBase(ClientTestsFixture fixture)
		{
			Fixture = fixture;
		}
	}
}
