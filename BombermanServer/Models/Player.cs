using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BombermanServer.Models
{
    public class Player
    {
        public int id { get; set; }
        public string connectionId { get; set; }
        public PointF position { get; set; }
        public int speedMultiplier { get; set; }
        private PlayerSprite sprite { get; set; }

        public Player()
        {
            // set the position and sprite based on connection id
            this.position = new PointF(0, 0);
            this.sprite = PlayerSprite.BLUE;
            this.speedMultiplier = 1;
        }
        
        public Player(int id, String connectionId) 
            : this()
        {
            this.id = id;
            this.connectionId = connectionId;
        }
    }

    public enum PlayerSprite
    {
        BLUE,
        GREEN,
        RED
    }
}
