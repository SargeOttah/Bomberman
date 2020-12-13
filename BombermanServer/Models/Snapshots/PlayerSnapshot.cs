using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using BombermanServer.Constants;
using BombermanServer.Models.Flyweight;
using SFML.Graphics;

namespace BombermanServer.Models.Snapshots
{
    public class PlayerSnapshot : ISnapshot
    {
        private readonly Player _player;

        private int Id { get; }
        private string ConnectionId { get; }
        private PointF Position { get; }
        private int SpeedMultiplier { get; }
        private PlayerFlyweight Flyweight { get; }

        public PlayerSnapshot(int id, string connectionId, PointF position, int speedMultiplier, PlayerFlyweight flyweight, Player player)
        {
            Id = id;
            ConnectionId = connectionId;
            Position = new PointF(position.X, position.Y);
            SpeedMultiplier = speedMultiplier;
            Flyweight = flyweight;
            this._player = player;
        }

        public void Restore()
        {
            _player.Id = Id;
            _player.ConnectionId = ConnectionId;
            _player.Position = new PointF(Position.X, Position.Y);
            _player.SpeedMultiplier = SpeedMultiplier;
            _player.Flyweight = Flyweight;
            _player.IsDead = false;
            Console.WriteLine($"Respawning player at: {Position.X} {Position.Y}");
            Console.WriteLine($"player is at: {_player.Position.X} {_player.Position.Y}");
        }

        public int GetRespawnTime()
        {
            return PlayerConstants.RespawnTime;
        }

        public int GetId()
        {
            return Id;
        }
    }
}
