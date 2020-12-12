using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BombermanServer.Models.Snapshots
{
    public class PlayerSnapshot
    {
        private readonly Player _player;

        private int Id { get; }
        private string ConnectionId { get; }
        private PointF Position { get; }
        private int SpeedMultiplier { get; }
        private PlayerSprite Sprite { get; }

        public PlayerSnapshot(int id, string connectionId, PointF position, int speedMultiplier, PlayerSprite sprite, Player player)
        {
            Id = id;
            ConnectionId = connectionId;
            Position = position;
            SpeedMultiplier = speedMultiplier;
            Sprite = sprite;
            this._player = player;
        }

        public void Restore()
        {
            _player.Id = Id;
            _player.ConnectionId = ConnectionId;
            _player.Position = Position;
            _player.SpeedMultiplier = SpeedMultiplier;
            _player.Sprite = Sprite;
        }
    }
}
