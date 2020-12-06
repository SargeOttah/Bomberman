using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Bomberman.Dto;
using log4net;

namespace Bomberman.HubHandler
{
    class UserHubClientProxy : IUserHubClient
    {
        private readonly IUserHubClient _client;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserHubClientProxy(IUserHubClient client)
        {
            this._client = client;
        }

        public void ClientConnected(PlayerDTO playerDTO, string[] map)
        {
            _log.Info("We have connected: " + playerDTO.ToString());
            _client.ClientConnected(playerDTO, map);
        }

        public void OnNewClientConnect(PlayerDTO playerDTO)
        {
            _log.Info("New client connected: " + playerDTO.ToString());
            _client.OnNewClientConnect(playerDTO);
        }

        public void RefreshPlayers(List<PlayerDTO> players)
        {
            if (players.Count > 4)
            {
                _log.Warn($"Refreshing {players.Count} client(s) data, player list over expected capacity.");
            }
            _client.RefreshPlayers(players);
        }

        public void OnNewBomb(BombDTO bombDTO)
        {
            _log.Info(bombDTO.ToString());
            _client.OnNewBomb(bombDTO);
        }

        public void OnBombExplosion(BombExplosionDTO bombExplosionDTO)
        {
            _log.Info("Bomb explosion: " + bombExplosionDTO.ToString());
            _client.OnBombExplosion(bombExplosionDTO);
        }

        public void RefreshMap(string[] map)
        {
            _client.RefreshMap(map);
        }
    }
}
