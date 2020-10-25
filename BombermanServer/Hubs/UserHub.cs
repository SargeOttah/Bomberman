using BombermanServer.Builders.PlayerBuilder;
using BombermanServer.Models;
using BombermanServer.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace BombermanServer.Hubs
{
    public class UserHub : Hub
    {
        private IPlayerService _playerService;
        private IMapService _mapService;

        public UserHub(IPlayerService playerService, IMapService mapService)
        {
            this._playerService = playerService;
            this._mapService = mapService;
            _mapService.LoadMap(0); // TODO: send map id from client side ant then load it?
        }

        public async Task SendMessage(string user, string message) // 'SendMessage' is a name that ClientSide sends requests to.
        {
            await Clients.Caller.SendAsync("ReceiveMessage", user, message); // 'ReceiveMessage' is a name that ClientSide listens to.
        }

        public async Task SendBombLocation(string user, PointF position) // 'SendMessage' is a name that ClientSide sends requests to.
        {
            // Wait for bomb signal
            await Clients.Caller.SendAsync("ReceiveBombLocation", user, position); 

            // Send signal of bomb creation
            //await Clients.All.SendAsync("ReceiveNewBomb", position);
            await Clients.AllExcept(this.Context.ConnectionId).SendAsync("ReceiveNewBomb", position);
        }

        public override async Task OnConnectedAsync()
        {
            int playerId = _playerService.GetEmptyId();
            Console.WriteLine("Client Connected:" + this.Context.ConnectionId + " " + playerId);
            var newPlayer = PlayerDirector.Build(playerId, Context.ConnectionId);

            if (!_playerService.AddPlayer(newPlayer))
            {
                // TODO: player limit exceeded
                Console.WriteLine("Player limit exceeded");
            }

            await Clients.Caller.SendAsync("ClientConnected", newPlayer, _mapService.GetMap()); // Sends back the newly created player and map to the owner
            Console.WriteLine(newPlayer.ToString());

            Console.WriteLine("Clients Count:" + _playerService.GetCount());
            

            if (_playerService.GetCount() > 1) // If there are other clients connected, notify them of the new player
            {
                await Clients.AllExcept(this.Context.ConnectionId).SendAsync("ReceiveNewClient", newPlayer);
            }

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // remove player
            Player player = _playerService.GetPlayer(this.Context.ConnectionId);
            if (_playerService.RemovePlayer(player))
            {
                Console.WriteLine($"Client {this.Context.ConnectionId} has disconnected.");
                // TODO: send a successful disconnect message to client
                // TODO: update other clients about the disconnect
            }
            else
            {

            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task RefreshPlayer(PointF playerPosition)
        {
            _playerService.GetPlayer(this.Context.ConnectionId).Position = playerPosition;
            List<Player> players = _playerService.GetPlayers();

            await Clients.Caller.SendAsync("RefreshPlayers", players);
        }

        public async Task RefreshMap(Dictionary<Point, char> changes) {
            await Clients.Caller.SendAsync("RefreshMap", _mapService.GetMap());
        }

        
    }
}
