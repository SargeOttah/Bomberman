﻿using BombermanServer.Builders.PlayerBuilder;
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

        public UserHub(IPlayerService playerService)
        {
            this._playerService = playerService;
        }

        public async Task SendMessage(string user, string message) // 'SendMessage' is a name that ClientSide sends requests to.
        {
            await Clients.Caller.SendAsync("ReceiveMessage", user, message); // 'ReceiveMessage' is a name that ClientSide listens to.
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

            await Clients.Caller.SendAsync("ClientConnected", newPlayer); // Sends back the newly created player to the owner
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

        public async Task Refresh(PointF playerPosition)
        {
            _playerService.GetPlayer(this.Context.ConnectionId).Position = playerPosition;
            List<Player> players = _playerService.GetPlayers();

            await Clients.Caller.SendAsync("RefreshPlayers", players);
        }
    }
}
