using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Bomberman.Dto
{
    public class PlayerDTO
    {
        public int id { get; set; }
        public string connectionId { get; set; }
        public PointF position { get; set; }
        public PlayerSprite sprite { get; set; }

        private static Dictionary<PlayerSprite, byte[]> spriteDict = new Dictionary<PlayerSprite, byte[]>
        {
            { PlayerSprite.BLUE, Properties.Resources.bluefront },
            { PlayerSprite.GREEN, Properties.Resources.greenfront },
            { PlayerSprite.RED, Properties.Resources.redfront }
        };

        public PlayerDTO()
        {

        }

        public PlayerDTO(int id, String connectionId, PointF position, PlayerSprite sprite)
        {
            this.id = id;
            this.connectionId = connectionId;
            this.position = position;
            this.sprite = sprite;
        }

        public Texture GetTexture()
        {
            return new Texture(spriteDict[this.sprite]);
        }

        public override string ToString()
        {
            return id.ToString() + " " + connectionId + " "+ position.X + "|" + position.Y + " " + this.sprite;
        }
    }

    public enum PlayerSprite
    {
        BLUE,
        GREEN,
        RED
    }
}
