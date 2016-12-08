#define DEBUG_SERVER
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client.UnitTests
{
    [SetUpFixture]
    public class TestSetup
    {
#if DEBUG_SERVER
        private const string ConnectionString = "";
        public const string BaseAddress = "http://localhost:62312/";
#else
        private const string ConnectionString = "Server=localhost;Port=3306;Database=SUGAR;Uid=root;Pwd=t0pSECr3t;Convert Zero Datetime=true;Allow Zero Datetime=true;Pooling=false;";
        public const string BaseAddress = "http://localhost:5000/";
#endif
        private Process _process;

        [OneTimeSetUp]
        public void SetUp()
        {
#if !DEBUG_SERVER
            KillPreviousServerInstances();
            DeleteDatabase();
#endif
            
#if DEBUG_SERVER
            while (!CanLogin())
            {
            }
#else
            StartServer();
#endif
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // Stop Server
            _process?.Kill();
            _process?.WaitForExit();
        }

        private bool CanLogin()
        {
            var client = new TestSUGARClient();
            var didLogin = false;
            var request = new AccountRequest
            {
                Name = "admin",
                Password = "admin",
                SourceToken = "SUGAR",
            };

            try
            {
                try
                {
                    client.Session.CreateAndLogin(request);
                }
                catch (Exception e)
                {
                    client.Session.Login(request);
                }

                didLogin = true;
            }
            catch (ClientException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }

            return didLogin;
        }

        private void KillPreviousServerInstances()
        {
            var processes = Process.GetProcessesByName("dotnet");
            foreach (var process in processes)
            {
                process.Kill();
                process.WaitForExit();
            }
        }

        private void StartServer()
        {
            // Build and Start Server
            var assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var rootDir = Directory.GetParent(assemblyDir).Parent.Parent.Parent.FullName;
            var serverDir = $"{rootDir}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}PlayGen.SUGAR.WebAPI";

            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = serverDir,
                FileName = "dotnet",
                Arguments = "run",
                UseShellExecute = false,
                EnvironmentVariables = { { "ASPNETCORE_ENVIRONMENT", "Development" } }
            };

            _process = Process.Start(startInfo);

            // Wait for server to startup
            // todo find less hackey way to do this - handle cases where server wont startup
            while (!CanLogin())
            {
            }
        }

        private void DeleteDatabase()
        {
            // Delete Database
            using (var connection = new MySqlConnection(ConnectionString))
            using (var command = new MySqlCommand("DROP DATABASE SUGAR", connection))
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
