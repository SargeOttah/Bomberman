using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace BombermanServer.Utils
{
    static class SpriteLoader
    {

        public static Sprite LoadSprite(byte[] imageBitmap, IntRect square, bool repeated = false)
        {
            var tmpSprite = new Sprite();

            var tmpRect = square;

            try
            {
                //tmpTexture
                var tmpTexture = new Texture(imageBitmap) { Repeated = repeated };
                tmpSprite = new Sprite(tmpTexture, tmpRect);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Sprite load error:\n{e}");
            }

            return tmpSprite; // unsafe?
        }
    }
}
