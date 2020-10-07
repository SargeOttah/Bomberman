using BombermanServer.Models;
using System.Drawing;

namespace BombermanServer.Builders.PlayerBuilder.ConcreteBuilders
{
    public class PlayerThreeBuilder : PlayerBuilder
    {
        public PlayerThreeBuilder(string connectionId) : base(connectionId) { }

        public override void BuildId()
        {
            Player.Id = 2;
        }
        public override void BuildPosition()
        {
            Player.Position = new PointF(0, 100);
        }

        public override void BuildSprite()
        {
            Player.Sprite = PlayerSprite.Green;
        }
    }
}
