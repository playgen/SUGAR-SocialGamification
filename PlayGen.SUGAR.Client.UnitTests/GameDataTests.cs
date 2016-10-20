using System;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.UnitTests
{
    public class GameDataTests
    {
        [Test]
        public void Test()
        {
            using (var client = new ClientWebSocket())
            {
                await client.ConnectAsync(new Uri(ClientAddress), CancellationToken.None);
                var orriginalData = new byte[0];
                await client.SendAsync(new ArraySegment<byte>(orriginalData), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }
    }
}
