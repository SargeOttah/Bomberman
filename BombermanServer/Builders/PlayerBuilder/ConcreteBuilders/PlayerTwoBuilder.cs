using BombermanServer.Models;
using System.Drawing;

namespace BombermanServer.Builders.PlayerBuilder.ConcreteBuilders
{
    public class PlayerTwoBuilder : PlayerBuilder
    {
        public PlayerTwoBuilder(string connectionId) : base(connectionId) { }

        public override void BuildId()
        {
            Player.Id = 1;
        }
        public override void BuildPosition()
        {
            Player.Position = new PointF(100, 0);
        }

        public override void BuildSprite()
        {
            Player.Sprite = PlayerSprite.Red;
        }
    }
}
