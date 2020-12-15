using BombermanServer.Constants;
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
            Player.Position = new PointF(MapConstants.tileSize * (MapConstants.mapWidth - 2) + MapConstants.tileSize / 2,
                                         MapConstants.tileSize + MapConstants.tileSize / 2);
        }

        public override void BuildSprite()
        {
            Player.Sprite = PlayerSprite.Red;
        }
    }
}
