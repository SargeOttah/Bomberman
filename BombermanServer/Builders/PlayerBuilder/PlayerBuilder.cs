using BombermanServer.Models;

namespace BombermanServer.Builders.PlayerBuilder
{
    public abstract class PlayerBuilder
    {
        public PlayerDTO Player { get; protected set; }

        protected PlayerBuilder(string connectionId)
        {
            Player = new PlayerDTO(connectionId);
        }

        public abstract void BuildId();
        public abstract void BuildPosition();
        public abstract void BuildSprite();
    }
}
