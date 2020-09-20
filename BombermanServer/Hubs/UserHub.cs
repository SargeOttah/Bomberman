using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace BombermanServer.Hubs
{
    public class UserHub : Hub
    {
        public async Task SendMessage(string user, string message) // 'SendMessage' is a name that ClientSide sends requests to.
        {
            // Anything other than '.All' does not work because ClientSide doesn't focus on a single window - pressing keyboard key triggers all active windows simultaneously.
            await Clients.All.SendAsync("ReceiveMessage", user, message); // 'ReceiveMessage' is a name that ClientSide listens to.
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Connected: {Context.ConnectionId}"); // You can test if connection works like that.
            await base.OnConnectedAsync();
        }
    }
}
