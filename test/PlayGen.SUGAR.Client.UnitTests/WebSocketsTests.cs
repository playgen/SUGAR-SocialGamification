using System;
using System.Diagnostics;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.UnitTests
{
    public class WebSocketsTests
    {
        [Test]
        public void EchoTest()
        {
            var timeoutMilliseconds = 30000; 
            var message = "TestMessage";

            var timer = Stopwatch.StartNew();

            using (var webSocket = new WebSocket(new Uri("ws://localhost:62312")))
            {
                webSocket.Connect();

                webSocket.SendString(message);
                
                while (timer.ElapsedMilliseconds < timeoutMilliseconds)
                {
                    var reply = webSocket.RecieveString();

                    if (reply != null)
                    {
                        Assert.AreEqual(message, reply);
                        break;
                    }

                    Assert.True(string.IsNullOrEmpty(webSocket.Error), $"Had error: {webSocket.Error}");
                }

                timer.Stop();

                Assert.LessOrEqual(timer.ElapsedMilliseconds, timeoutMilliseconds, $"Test forced process timeout after {timeoutMilliseconds} milliseconds.");
            }
        }

        [Test]
        public void GameData()
        {
            // todo
            // create web socket client commands to send gamedata
                // compose web socket command with reqyest object

            // intercept on server
            // route to correct api call - find using reflection
            // cache api route from command to methodinfo

            // refactor echo test so connection can be tested, have echo web api controller
        }
    }
}
