using BombermanServer.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BombermanServer.Services.Impl
{
    public class EnemyMovementService : IEnemyMovementService
    {
        private readonly IHubContext<UserHub> _hubContext;
        private Timer _updateTimer;

        public EnemyMovementService(IHubContext<UserHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task UpdateGhostMovement()
        {
            var x = 0;
            var y = 480;
            _updateTimer = new Timer(async _ =>
            {
                Console.WriteLine(x);
                x++;
                await _hubContext.Clients.All.SendAsync("RefreshEnemies", x.ToString(), y.ToString());

            }, null, 0, 500);
        }
    }
}
