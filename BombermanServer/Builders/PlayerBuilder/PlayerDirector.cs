using BombermanServer.Builders.PlayerBuilder.ConcreteBuilders;
using BombermanServer.Models;

namespace BombermanServer.Builders.PlayerBuilder
{
    public static class PlayerDirector
    {
        public static Player Build(int playerId, string connectionId)
        {
            var builder = GetBuilder(playerId, connectionId);

            builder.BuildId();
            builder.BuildPosition();
            builder.BuildSprite();

            return builder.Player;
        }

        private static PlayerBuilder GetBuilder(int playerId, string connectionId)
        {
            return playerId switch
            {
                0 => new PlayerOneBuilder(connectionId),
                1 => new PlayerTwoBuilder(connectionId),
                2 => new PlayerThreeBuilder(connectionId),
                _ => new PlayerFourBuilder(connectionId)
            };
        }
    }
}
