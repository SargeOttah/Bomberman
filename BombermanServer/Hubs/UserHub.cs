using BombermanServer.Builders.PlayerBuilder;
using BombermanServer.Models;
using BombermanServer.Services;
using BombermanServer.Configurations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Linq;
using SFML.System;



namespace BombermanServer.Hubs
{
    public class UserHub : Hub
    {
        private IPlayerService _playerService;
        private IMapService _mapService;
        private IBombService _bombService;

        public UserHub(IPlayerService playerService, IBombService bombService, IEnumerable<IMapService> mapServices, IOptions<MapConfiguration> settings)
        {
            this._playerService = playerService;
            this._bombService = bombService;
            this._mapService = mapServices.FirstOrDefault(h => h.GetServiceName() == settings.Value.CurrentMapLoader);
        }

        public async Task SendMessage(string user, string message) // 'SendMessage' is a name that ClientSide sends requests to.
        {
            await Clients.Caller.SendAsync("ReceiveMessage", user, message); // 'ReceiveMessage' is a name that ClientSide listens to.
        }

        public async Task OnBombPlace(Bomb bomb) // creating a new bomb
        {
            Console.WriteLine(bomb.ToString());
            _bombService.Add(bomb);
            await Clients.All.SendAsync("ReceiveNewBomb", bomb);
        }

        public override async Task OnConnectedAsync()
        {
            int playerId = _playerService.GetEmptyId();
            Console.WriteLine("Client Connected:" + this.Context.ConnectionId + " " + playerId);
            var newPlayer = PlayerDirector.Build(playerId, Context.ConnectionId);

            if (_playerService.GetCount() == 0)
            { // load map for first player
                _mapService.LoadMap(); // TODO: send map id from client side ant then load it?
            }

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
    }
}
