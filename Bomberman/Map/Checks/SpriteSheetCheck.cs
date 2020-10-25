using SFML.Graphics;
using System;

namespace Bomberman.Map.Checks
{
    public class SpriteSheetCheck
    {
        int spriteSize;

        public SpriteSheetCheck(int spriteSize){
            this.spriteSize = spriteSize;
        }

        public bool IsValidSpriteSheet(Texture spriteSheet) {
            double spriteColumnCount = unchecked((int)spriteSheet.Size.X) / spriteSize;
            double spriteRowCount = unchecked((int)spriteSheet.Size.X) / spriteSize;
            Console.WriteLine(spriteColumnCount + " " + spriteRowCount);
            return (spriteColumnCount % 1 == 0 && spriteRowCount % 1 == 0);
        }

        public bool IsValidTile(char tileIndex, Texture spriteSheet) {
            int spriteColumnCount = unchecked((int)spriteSheet.Size.X) / spriteSize;
            int spriteRowCount = unchecked((int)spriteSheet.Size.Y) / spriteSize;
            int tileCount = spriteColumnCount * spriteRowCount;
            return !Char.IsDigit(tileIndex) || (tileIndex >= 0 && tileIndex < tileCount);
        }
    }
}