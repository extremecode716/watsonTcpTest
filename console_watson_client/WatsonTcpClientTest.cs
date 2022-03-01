using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WatsonTcp;

namespace console_watson_client
{
    public class WatsonTcpClientTest : IDisposable
    {
        private WatsonTcpClient client;

        public WatsonTcpClientTest()
        {
            client = null;
        }

        ~WatsonTcpClientTest()
        {
            Dispose();
        }

        public void CreateClient(string serverIp, int serverPort)
        {
            Dispose();

            client = new WatsonTcpClient(serverIp, serverPort);
            client.Events.ServerConnected += ServerConnected;
            client.Events.ServerDisconnected += ServerDisconnected;
            client.Events.MessageReceived += MessageReceived;

            client.Keepalive.EnableTcpKeepAlives = true;
            client.Keepalive.TcpKeepAliveInterval = 5;      // seconds to wait before sending subsequent keepalive
            client.Keepalive.TcpKeepAliveTime = 5;          // seconds to wait before sending a keepalive
            client.Keepalive.TcpKeepAliveRetryCount = 5;    // number of failed keepalive probes before terminating connection
        }

        public void Connect()
        {
            client?.Connect();
        }

        public void Disconnect()
        {
            client?.Disconnect();
        }

        public bool Connected()
        {
            return client?.Connected ?? false;
        }

        public void Send(string data, Dictionary<object, object> metadata = null)
        {
            client?.Send(data, metadata);
            Console.WriteLine("Message Send : " + data);
        }

        public async void SendAsync(string data, Dictionary<object, object> metadata = null, CancellationToken token = default(CancellationToken))
        {
            await client?.SendAsync(data, metadata, token);
            Console.WriteLine("Message Send : " + data);
        }

        public void SendAndWait(int timeoutMs, string data, Dictionary<object, object> metadata = null)
        {
            try
            {
                client?.SendAndWait(timeoutMs, data, metadata);
            }
            catch (TimeoutException)
            {
                throw;
            }
        }

        public void Dispose()
        {
            client?.Disconnect();
            client?.Dispose();
            client = null;
        }

        private static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            string response = Encoding.UTF8.GetString(args.Data);
            Console.WriteLine("Message from " + args.IpPort + ": " + response);
        }

        private static void ServerConnected(object sender, EventArgs args)
        {
            Console.WriteLine("Server connected");
        }

        private static void ServerDisconnected(object sender, EventArgs args)
        {
            Console.WriteLine("Server disconnected");
        }

        private static SyncResponse SyncRequestReceived(SyncRequest req)
        {
            return new SyncResponse(req, "Hello back at you!");
        }
    }
}
