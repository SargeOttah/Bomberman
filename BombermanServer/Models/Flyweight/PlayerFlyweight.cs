namespace BombermanServer.Models.Flyweight
{
    public class PlayerFlyweight
    {
        public PlayerSprite Sprite { get; set; }

        public PlayerFlyweight(PlayerSprite sprite)
        {
            Sprite = sprite;
        }
    }

    public enum PlayerSprite
    {
        Blue = 0,
        Green = 1,
        Red = 2
    }
}
