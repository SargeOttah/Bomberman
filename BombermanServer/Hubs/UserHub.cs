using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BombermanServer.Hubs
{
    public class UserHub : Hub
    {
        public async Task SendMessage(string user, string message) // 'SendMessage' is a name that ClientSide sends requests to.
        {
            await Clients.Others.SendAsync("ReceiveMessage", user, message); // 'ReceiveMessage' is a name that ClientSide listens to.
        }
    }
}
