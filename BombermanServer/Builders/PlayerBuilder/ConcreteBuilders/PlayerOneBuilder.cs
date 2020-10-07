using BombermanServer.Models;
using System.Drawing;

namespace BombermanServer.Builders.PlayerBuilder.ConcreteBuilders
{
    public class PlayerOneBuilder : PlayerBuilder
    {
        public PlayerOneBuilder(string connectionId) : base(connectionId) { }

        public override void BuildId()
        {
            Player.Id = 0;
        }

        public override void BuildPosition()
        {
            Player.Position = new PointF(0, 0);
        }

        public override void BuildSprite()
        {
            Player.Sprite = PlayerSprite.Blue;
        }
    }
}
