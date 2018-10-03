using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace server.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public override Task OnConnected()
        {
            Clients.All.Notify($"Connected :{Context.ConnectionId.ToString()}");
            return base.OnConnected();
        }
        public override Task OnReconnected()
        {
            Clients.All.Notify($"Reconnected :{Context.ConnectionId.ToString()}");
            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.All.Notify($"Disconnected :{Context.ConnectionId.ToString()}");
            return base.OnDisconnected(stopCalled);
        }
        public void Send(string name, string message)
        {
            Clients.Others.Message($"{name}: {message}");
        }
    }
}