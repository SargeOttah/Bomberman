using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BombermanServer.Models.Snapshots
{
    public interface ISnapshot
    {
        public void Restore();
        // Returns time needed to respawn in seconds
        public int GetRespawnTime();

        public int GetId();
    }
}
