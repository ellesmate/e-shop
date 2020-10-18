using Microsoft.AspNetCore.SignalR;

namespace Shop.UI.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;
    }
}
