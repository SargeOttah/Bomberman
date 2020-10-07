using BombermanServer.Models;

namespace BombermanServer.Builders.PlayerBuilder
{
    public abstract class PlayerBuilder
    {
        public Player Player { get; protected set; }

        protected PlayerBuilder(string connectionId)
        {
            Player = new Player(connectionId);
        }

        public abstract void BuildId();
        public abstract void BuildPosition();
        public abstract void BuildSprite();
    }
}
