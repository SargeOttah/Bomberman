using System;
using System.Collections.Generic;
using System.Linq;
using Bomberman.Dto;

namespace Bomberman.HubHandler
{
    public class UserHubClient : IUserHubClient
    {
        private readonly GameApplication _game;

        public UserHubClient()
        {
            _game = GameApplication.GetInstance();
        }

        // Called when this client connects to the server, receives the player information
        public void ClientConnected(PlayerDTO playerDTO, string[] map)
        {
            Console.WriteLine("We have connected");
            Console.WriteLine(playerDTO.ToString());
            if (!_game.tileMapFacade.SetupTileMap(map))
            {
                Console.WriteLine("Invalid map");
            }
            _game.mainPlayer = new Player(playerDTO);
        }

        // Called when a new client (except the current one) connects to the server, receives the other players information
        public void OnNewClientConnect(PlayerDTO playerDTO)
        {
            var newPlayer = new Player(playerDTO);
            _game.otherPlayers.Add(newPlayer);
        }

        // Called to refresh information about the current players on the server
        public void RefreshPlayers(List<PlayerDTO> players)
        {
            PlayerDTO main = players.First(p => p.connectionId.Equals(_game.mainPlayer.connectionId, StringComparison.Ordinal));
            List<PlayerDTO> others = players.Where(p => !p.connectionId.Equals(_game.mainPlayer.connectionId, StringComparison.Ordinal)).ToList();

            _game.mainPlayer.UpdateStats(main);
            foreach (PlayerDTO pNew in others)
            {
                Player p = _game.otherPlayers.Find(p => string.Equals(p.connectionId, pNew.connectionId, StringComparison.Ordinal));
                if (p != null)
                {
                    p.UpdateStats(pNew);
                }
                else
                {
                    _game.otherPlayers.Add(new Player(pNew));
                }
            }
        }

        // Called when a new bomb is created
        public void OnNewBomb(BombDTO bombDTO)
        {
            _game.mainPlayer.AddBomb(bombDTO);
        }

        // Called when a bomb explosion event is sent
        public void OnBombExplosion(BombExplosionDTO bombExplosionDTO)
        {
            _game.mainPlayer.CreateExplosion(bombExplosionDTO);
        }

        // Used to refresh map
        public void RefreshMap(string[] map)
        {
            _game.tileMapFacade.UpdateTileMap(map);
        }
    }
}