using BombermanServer.Constants;
using BombermanServer.Models.Flyweight;
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
            Player.Position = new PointF(MapConstants.tileSize + MapConstants.tileSize / 2,
                                         MapConstants.tileSize + MapConstants.tileSize / 2);
        }

        public override void BuildSprite()
        {
            Player.Flyweight = PlayerFlyweightFactory.GetPlayerFlyweight(PlayerSprite.Blue);
        }
    }
}
