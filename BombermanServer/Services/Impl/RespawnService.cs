using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BombermanServer.Models.Snapshots;

namespace BombermanServer.Services.Impl
{
    public class RespawnService : IRespawnService
    {
        private readonly IPlayerService _playerService;

        public RespawnService(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async void QueueRespawn(ISnapshot snapshot)
        {
            await Task.Delay(TimeSpan.FromSeconds(snapshot.GetRespawnTime()));
            snapshot.Restore();
        }
    }
}
