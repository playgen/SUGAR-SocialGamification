using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Client
{
    public class WebSocket : IDisposable
    {
        private readonly Uri _url;
        private readonly Queue<byte[]> _messages = new Queue<byte[]>();

        private WebSocketSharp.WebSocket _socket;

        public bool IsConnected { get; private set; }
        public string Error { get; private set; }

        public WebSocket(Uri url)
        {
            _url = url;
            var protocol = _url.Scheme;

            if (!protocol.Equals("ws") && !protocol.Equals("wss"))
            {
                throw new ArgumentException("Unsupported protocol: " + protocol);
            }
        }

        public void SendString(string str)
        {
            Send(Encoding.UTF8.GetBytes(str));
        }

        public string RecieveString()
        {
            var retval = Recieve();

            return retval == null 
                ? null
                : Encoding.UTF8.GetString(retval);
        }

        public void Connect()
        {
            _socket = new WebSocketSharp.WebSocket(_url.ToString());

            _socket.OnMessage += (sender, e) => _messages.Enqueue(e.RawData);
            _socket.OnOpen += (sender, e) => IsConnected = true;
            _socket.OnClose += (sender, e) => IsConnected = false;
            _socket.OnError += (sender, e) => Error = e.Message;

            _socket.Connect();
        }

        public void Send(byte[] buffer)
        {
            _socket.Send(buffer);
        }

        public byte[] Recieve()
        {
            return _messages.Count == 0 
                ? null
                : _messages.Dequeue();
        }

        
        public void Dispose()
        {
            _socket.Close();
        }
    }
}
