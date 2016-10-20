using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace PlayGen.SUGAR.WebAPI.WebSockets
{
    public class SocketHandler
    {
        public const int BufferSize = 4096;

        private readonly WebSocket _socket;

        private SocketHandler(WebSocket socket)
        {
            _socket = socket;
        }

        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(Acceptor);
        }

        private async Task EchoLoop()
        {
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);

            while (_socket.State == WebSocketState.Open)
            {
                var incoming = await _socket.ReceiveAsync(seg, CancellationToken.None);
                var outgoing = new ArraySegment<byte>(buffer, 0, incoming.Count);
                await _socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private static async Task Acceptor(HttpContext context, Func<Task> n)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var handler = new SocketHandler(socket);
            await handler.EchoLoop();
        }
    }
}
