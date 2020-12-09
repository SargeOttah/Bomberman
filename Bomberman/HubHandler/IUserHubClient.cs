using System.Collections.Generic;
using Bomberman.Dto;
using NUnit.Framework;

namespace Bomberman.HubHandler
{
    public interface IUserHubClient
    {
        void ClientConnected(PlayerDTO playerDTO, string[] map);
        void OnNewClientConnect(PlayerDTO playerDTO);
        void RefreshPlayers(List<PlayerDTO> players);
        void OnNewBomb(BombDTO bombDTO);
        void OnBombExplosion(BombExplosionDTO bombExplosionDTO);
        void RefreshMap(string[] map);
    }
}