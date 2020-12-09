using BombermanServer.Models;
using System.Collections.Generic;

namespace BombermanServer.Services.Strategies
{
    public interface IPlayerEmptyIdStrategy
    {
        int GetEmptyId(IEnumerable<PlayerDTO> players);
    }
}
