using BombermanServer.Constants;
using BombermanServer.Models;
using System.Drawing;

namespace BombermanServer.Builders.PlayerBuilder.ConcreteBuilders
{
    public class PlayerFourBuilder : PlayerBuilder
    {
        public PlayerFourBuilder(string connectionId) : base(connectionId) { }

        public override void BuildId()
        {
            Player.Id = 3;
        }

        public override void BuildPosition()
        {
            Player.Position = new PointF(MapConstants.tileSize * (MapConstants.mapWidth - 2) + MapConstants.tileSize / 2,
                                         MapConstants.tileSize * (MapConstants.mapHeight - 2) + MapConstants.tileSize / 2);
        }

        public override void BuildSprite()
        {
            Player.Sprite = PlayerSprite.Blue; // NEED NEW SPRITE - this one is same as Player One!!!
        }
    }
}
