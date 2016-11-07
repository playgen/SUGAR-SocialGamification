using System.Diagnostics;
using NUnit.Framework;
using PlayGen.SUGAR.Client;
using MySql.Data.MySqlClient;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class TestSUGARClient : SUGARClient
    {
        public TestSUGARClient() : base(TestSetup.BaseAddress)
		{
		}
	}
}
