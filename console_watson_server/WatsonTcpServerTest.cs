using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WatsonTcp;

namespace console_watson_server
{
    public class WatsonTcpServerTest : IDisposable
    {
        private WatsonTcpServer server;

        public WatsonTcpServerTest()
        {
        }

        ~WatsonTcpServerTest()
        {
            Dispose();
        }

        public void CreateServer(string listenerIp, int listenerPort)
        {
            Dispose();

            server = new WatsonTcpServer(listenerIp, listenerPort);
            server.Events.ClientConnected += ClientConnected;
            server.Events.ClientDisconnected += ClientDisconnected;
            server.Events.MessageReceived += MessageReceived;
            server.Events.ServerStarted += ServerStarted;
            server.Events.ServerStopped += ServerStopped;

            server.Callbacks.SyncRequestReceived = SyncRequestReceived;
        }

        public void Start()
        {
            server?.Start();
        }

        public void Stop()
        {
            server?.Stop();
        }

        public IEnumerable<string> GetClients()
        {
            return server?.ListClients();
        }

        public void Send(string ipPort, string data, Dictionary<object, object> metadata = null)
        {
            server?.Send(ipPort, data, metadata);
        }

        public async void SendAsync(string ipPort, string data, Dictionary<object, object> metadata = null, int start = 0, CancellationToken token = default(CancellationToken))
        {
            await server?.SendAsync(ipPort, data, metadata, start, token);
        }

        public void SendAndWait(int timeoutMs, string ipPort, string data, Dictionary<object, object> metadata = null)
        {
            try
            {
                server?.SendAndWait(timeoutMs, ipPort, data, metadata);
            }
            catch (TimeoutException)
            {
                throw;
            }
        }

        public void Dispose()
        {
            server?.DisconnectClients();
            server?.Dispose();
            server = null;
        }

        private static void ClientConnected(object sender, ConnectionEventArgs args)
        {
            Console.WriteLine("Client connected: " + args.IpPort);
        }

        private static void ClientDisconnected(object sender, DisconnectionEventArgs args)
        {
            Console.WriteLine("Client disconnected: " + args.IpPort + ": " + args.Reason.ToString());
        }

        private static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message from " + args.IpPort + ": " + Encoding.UTF8.GetString(args.Data));
        }

        private static SyncResponse SyncRequestReceived(SyncRequest req)
        {
            return new SyncResponse(req, "Hello back at you!");
        }

        private static void ServerStarted(object sender, EventArgs args)
        {
            Console.WriteLine("Server started");
        }

        private static void ServerStopped(object sender, EventArgs args)
        {
            Console.WriteLine("Server stopped");
        }
    }
}
