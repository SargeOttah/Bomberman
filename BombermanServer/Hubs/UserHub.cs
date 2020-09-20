using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BombermanServer.Hubs
{
    public class UserHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            Console.Write("Got message");
            await Clients.All.SendAsync("SendMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            Console.WriteLine(Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }
    }
}
