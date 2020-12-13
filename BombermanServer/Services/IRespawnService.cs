using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BombermanServer.Models.Snapshots;

namespace BombermanServer.Services
{
    public interface IRespawnService
    {
        public void QueueRespawn(ISnapshot snapshot);
    }
}
