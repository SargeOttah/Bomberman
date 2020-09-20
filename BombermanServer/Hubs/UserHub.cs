using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BombermanServer.Hubs
{
    public class UserHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
