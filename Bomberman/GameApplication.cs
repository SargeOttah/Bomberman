﻿using Microsoft.AspNetCore.SignalR.Client;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.IO;

namespace Bomberman
{
    internal class GameApplication
    {
        private static readonly GameApplication Instance = new GameApplication();

        private static RenderWindow _renderWindow;
        private static Texture _backgroundTexture;

        // TODO: Sprite arrays/lists to load
        private static Sprite _backgroundSprite;
        private static Sprite _playerSprite;
        private static readonly uint[] VideoResolution = { 800, 600 };
        private const string WindowTitle = "Bomberman v0.01";

        private static HubConnection _userHubConnection;

        public static GameApplication GetInstance()
        {
            return Instance;
        }

        private static void ConfigureHubConnection()
        {
            _userHubConnection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:5001/user-hub")
                    .Build();

            _userHubConnection.StartAsync().Wait();
        }

        public void Run()
        {
            ConfigureHubConnection();
            // TODO: Selector
            // VideoResolution = new uint[] { 500, 500 };   // Graphics resolution
            // VideoResolution = new uint[] { 800, 600 };   // Graphics resolution
            // VideoResolution = new uint[] { 1366, 768 };  // Graphics resolution
            // VideoResolution = new uint[] { 1280, 720 };  // Graphics resolution
            // VideoResolution = new uint[] { 1920, 1080 }; // Graphics resolution

            _renderWindow = CreateRenderWindow(Styles.Default);
            LoadGround();
            _renderWindow.SetActive();

            // Fetching sprites
            // TODO: autonomic loading
            _playerSprite = LoadSprite("Sprites\\Player\\Red\\redfront.png", new IntRect(0, 0, 19, 32));
            _playerSprite.Position = new Vector2f((float)VideoResolution[0] / 2, (float)VideoResolution[1] / 2);
            _playerSprite.Scale = new Vector2f(3, 3);

            while (_renderWindow.IsOpen)
            {
                _renderWindow.DispatchEvents(); // event handler to processes keystrokes/mouse movements
                _renderWindow.Clear();
                _renderWindow.Draw(_backgroundSprite);
                _renderWindow.Draw(_playerSprite);
                _renderWindow.Display(); // update screen

                InputControl();
            }
        }
        public void InputControl() // TODO: improve movement overlap, etc
        {
            var totalMovement = new Vector2f(0, 0);

            var tempPos = _playerSprite.Position;
            const int speed = 4;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                totalMovement.Y -= speed;
                _userHubConnection.InvokeAsync("SendMessage", "Asd", "asd").Wait();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                totalMovement.X -= speed;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                totalMovement.Y += speed;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                totalMovement.X += speed;
            }

            _playerSprite.Position = new Vector2f(tempPos.X + totalMovement.X, tempPos.Y + totalMovement.Y);
        }
        private static void OnClose(object sender, EventArgs e)
        {
            var renderWindow = (RenderWindow)sender;
            renderWindow.Close();
        }

        private static RenderWindow CreateRenderWindow(Styles windowStyle)
        {
            var videoMode = new VideoMode(VideoResolution[0], VideoResolution[1]);
            var renderWindow = new RenderWindow(videoMode, WindowTitle, windowStyle);

            renderWindow.Closed += OnClose;
            renderWindow.SetMouseCursorVisible(true);
            renderWindow.SetFramerateLimit(60);

            Console.WriteLine($"Resolution: {videoMode.Width}x{videoMode.Height}");
            return renderWindow;
        }
        private Sprite LoadSprite(string path, IntRect square) // TODO: improve
        {
            var tmpSprite = new Sprite();

            var tmpRect = square;

            try
            {
                //tmpTexture
                var tmpTexture = new Texture(getRelativePath(path)) { /*Repeated = false */};
                tmpSprite = new Sprite(tmpTexture, tmpRect);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Sprite load error:\n{e}");
            }

            return tmpSprite; // unsafe?
        }
        private void LoadGround()
        {
            try
            {
                _backgroundTexture = new Texture(getRelativePath("Sprites\\Ground\\Title_Image.png")) { Repeated = true };
                Console.WriteLine(getRelativePath());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Sprite load error:\n{e}");
            }

            // left, top, width, length
            var square = new IntRect(0, 0, (int)VideoResolution[0], (int)VideoResolution[1]);
            _backgroundSprite = new Sprite(_backgroundTexture, square);
        }
        /// <summary>
        /// Get relative path from executable dir ~\Bomberman\
        /// </summary>
        /// <param name="myPath"></param>
        /// <returns></returns>
        private string getRelativePath(string myPath = "")
        {
            var fullPath = "";
            var relPath = myPath;

            try
            {
                var curPath = Directory.GetCurrentDirectory();
                fullPath = Path.GetFullPath(Path.Combine(curPath, @"..\..\..\", relPath));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Current directory load error: {e}");
            }

            return fullPath;
        }
    }
}
