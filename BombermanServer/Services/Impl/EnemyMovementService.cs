using BombermanServer.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading;

namespace BombermanServer.Services.Impl
{
    public class EnemyMovementService : IEnemyMovementService
    {
        private readonly IPlayerService _playerService;
        private readonly IHubContext<UserHub> _hubContext;

        public EnemyMovementService(IHubContext<UserHub> hubContext, IPlayerService playerService)
        {
            _hubContext = hubContext;
            _playerService = playerService;

            UpdateGhostMovement();
        }

        public void UpdateGhostMovement()
        {
            var x = 0;
            var y = 480;
            new Timer(async _ =>
            {
                if (_playerService.GetPlayers().Any())
                {
                    x += 10;
                    await _hubContext.Clients.All.SendAsync("RefreshEnemies", x.ToString(), y.ToString());
                }
            }, null, 0, 100);
        }
    }
}
