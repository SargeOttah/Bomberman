using BombermanServer.Constants;
using BombermanServer.Models.Flyweight;
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
            Player.Position = new PointF(MapConstants.tileSize + MapConstants.tileSize / 2,
                                         MapConstants.tileSize * (MapConstants.mapHeight - 2) + MapConstants.tileSize / 2);
        }

        public override void BuildSprite()
        {
            Player.Flyweight = PlayerFlyweightFactory.GetPlayerFlyweight(PlayerSprite.Green);
        }
    }
}
