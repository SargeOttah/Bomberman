using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Bomberman.Dto
{
    public class PlayerDTO
    {
        public int id { get; set; }
        public string connectionId { get; set; }
        public PointF position { get; set; }
        public PlayerFlyweight Flyweight { get; set; }
        public bool IsDead { get; set; }

        private static Dictionary<PlayerSprite, byte[]> spriteDict = new Dictionary<PlayerSprite, byte[]>
        {
            { PlayerSprite.BLUE, Properties.Resources.bluefront },
            { PlayerSprite.GREEN, Properties.Resources.greenfront },
            { PlayerSprite.RED, Properties.Resources.redfront }
        };

        public PlayerDTO()
        {

        }
        public PlayerDTO(int id, PointF position, PlayerFlyweight flyweight)
        {
            this.id = id;
            this.position = position;
            this.Flyweight = flyweight;
        }


        public PlayerDTO(int id, String connectionId, PointF position, PlayerFlyweight flyweight)
        {
            this.id = id;
            this.connectionId = connectionId;
            this.position = position;
            this.Flyweight = flyweight;
        }

        public Texture GetTexture()
        {
            return new Texture(spriteDict[this.Flyweight.Sprite]);
        }

        public override string ToString()
        {
            return id.ToString() + " " + connectionId + " " + position.X + "|" + position.Y + " " + this.Flyweight.Sprite;
        }
    }
}
