using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace clientDOTNET
{
    class Program
    {
        private static SynchronizationContext Context;
        private const string ADDRESS = "http://localhost:51030/signalr";
        static void Main(string[] args)
        {
            bool terminate = false;
            Console.WriteLine("Input UserName : ");
            string userName = Console.ReadLine();
            Context = SynchronizationContext.Current;
            var hubConnection = new HubConnection(ADDRESS, useDefaultUrl: false);
            IHubProxy chatHubProxy = hubConnection.CreateHubProxy("chatHub");
            chatHubProxy["userName"] = userName;
            chatHubProxy["computerName"] = "basilin-PC";
            var notifyHandler = chatHubProxy.On("notify", (message) =>
                Console.WriteLine(message)
            );
            var messageHandler = chatHubProxy.On("message", (message) =>
                Console.WriteLine($": {message}")
            );

            /**
             * Life Cycle Events.
             * --------------------
             * Received: Raised when any data is received on the connection. Provides the received data.
             * ConnectionSlow: Raised when the client detects a slow or frequently dropping connection.
             * Reconnecting: Raised when the underlying transport begins reconnecting.
             * Reconnected: Raised when the underlying transport has reconnected.
             * StateChanged: Raised when the connection state changes. Provides the old state and the new state.
             * Closed: Raised when the connection has disconnected.
             **/
            hubConnection.Received += HubConnection_Received;
            hubConnection.ConnectionSlow += HubConnection_ConnectionSlow;
            hubConnection.Reconnecting += HubConnection_Reconnecting;
            hubConnection.Reconnected += HubConnection_Reconnected;
            hubConnection.StateChanged += HubConnection_StateChanged;
            hubConnection.Closed += HubConnection_Closed;

            /**
             * Start Connection
             **/
            hubConnection.Start().Wait();

            chatHubProxy.Invoke("send", "ConsoleApp", "Hello world").Wait();

            while (!terminate)
            {
                string message = Console.ReadLine();
                chatHubProxy.Invoke("send", userName, message).Wait();
                if (message == "stop" || message == "exit") terminate = true;
            }

            notifyHandler.Dispose();
            messageHandler.Dispose();
            hubConnection.Stop();
        }

        #region Life Time Events
        private static void HubConnection_Closed()
        {
            Console.WriteLine("HubConnection_Closed");
        }

        private static void HubConnection_StateChanged(StateChange obj)
        {
            //Console.WriteLine($"HubConnection_StateChanged from {obj.OldState} to {obj.NewState}");
        }

        private static void HubConnection_Reconnected()
        {
            Console.WriteLine("HubConnection_Reconnected");
        }

        private static void HubConnection_Reconnecting()
        {
            Console.WriteLine("HubConnection_Reconnecting");
        }

        private static void HubConnection_ConnectionSlow()
        {
            Console.WriteLine("HubConnection_ConnectionSlow");
        }

        private static void HubConnection_Received(string obj)
        {
            Console.WriteLine($"HubConnection_Received : {obj}");
        }
        #endregion
    }
}
