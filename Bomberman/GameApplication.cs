using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices.ComTypes;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Numerics;

namespace Bomberman
{
    class GameApplication
    { 
        private static readonly GameApplication _instance = new GameApplication();

        RenderWindow window;
        Texture backgroundTexture;
        Sprite backgroundSprite;
        public static GameApplication GetInstance()
        {
            return _instance;
        }

        public void Run()
        {
            window = CreateRenderWindow(Styles.Close);
            Vector2f winSize = window.GetView().Size;
            LoadGround();
            //Clock clock = new Clock();

            while (window.IsOpen)
            {
                //Time deltaTime = clock.Restart();
                window.Display();
                window.Clear();
                //this.ProccesKeyboardInput(deltaTime);
                window.Draw(backgroundSprite);
            }
        }

        public RenderWindow CreateRenderWindow(Styles windowStyle)
        {
            VideoMode videoMode = new VideoMode(1280, 720);
            RenderWindow window = new RenderWindow(videoMode, "Bomberman v0.01", windowStyle);
            window.SetMouseCursorVisible(false);
            window.SetFramerateLimit(120);

            //BindWindowEvents(window);

            return window;
        }

        private void LoadGround()
        {
            backgroundTexture = new Texture("D:/Uni/Bomberman/Bomberman/Bomberman/Sprites/Ground/Title_Image.png") {Repeated = true};
            // left, top, width, length
            IntRect square = new IntRect(0, 0, 1280, 720);
            backgroundSprite = new Sprite(backgroundTexture, square);
        }
    }
}
