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
        public PlayerSprite sprite { get; set; }

        public Player()
        {
            this.speedMultiplier = 1;
        }
        
        public Player(int id, String connectionId) 
            : this()
        {
            this.id = id;
            this.connectionId = connectionId;
            ConfigurePlayer();
        }

        private void ConfigurePlayer()
        {
            switch (this.id)
            {
                case 0:
                    this.position = new PointF(0, 0);
                    this.sprite = PlayerSprite.BLUE;
                    break;
                case 1:
                    this.position = new PointF(100, 0);
                    this.sprite = PlayerSprite.RED;
                    break;
                case 2:
                    this.position = new PointF(0, 100);
                    this.sprite = PlayerSprite.GREEN;
                    break;
                case 3:
                    this.position = new PointF(100, 100);
                    this.sprite = PlayerSprite.BLUE; // need 4th sprite
                    break;
                default:
                    Console.WriteLine("this should never appear in console:)");
                    break;
            }
        }

        public override string ToString()
        {
            return id.ToString() + " " + connectionId + " " + position.X + "|" + position.Y + " " + this.sprite;
        }
    }

    public enum PlayerSprite
    {
        BLUE,
        GREEN,
        RED
    }
}
