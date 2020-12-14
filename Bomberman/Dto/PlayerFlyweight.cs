namespace Bomberman.Dto
{
    public class PlayerFlyweight
    {
        public PlayerSprite Sprite { get; set; }
    }

    public enum PlayerSprite
    {
        BLUE,
        GREEN,
        RED
    }
}
