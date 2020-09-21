using Microsoft.AspNetCore.SignalR.Client;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Bomberman
{
    internal class GameApplication
    {
        private static readonly GameApplication Instance = new GameApplication();

        private static RenderWindow _renderWindow;
        private static Texture _backgroundTexture;

        // TODO: Sprite arrays/lists to load
        private static Sprite _backgroundSprite;
        private static Sprite _boxWall;
        private static readonly uint[] VideoResolution = { 800, 600 };
        private const string WindowTitle = "Bomberman v0.01";

        //player init
        Player mainPlayer = new Player();
        IntRect playerTexture = new IntRect(0, 0, 19, 32);

        private static HubConnection _userHubConnection;

        public static GameApplication GetInstance()
        {
            return Instance;
        }

        private static void ConfigureHubConnections()
        {
            _userHubConnection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:5001/user-hub")
                    .Build();

            _userHubConnection.StartAsync().Wait();

            _userHubConnection.On("ReceiveMessage", (string user, string message) => Console.WriteLine($"{user}: {message}")); // Demo listener.

        }

        public void Run()
        {
            //ConfigureHubConnections();
            // TODO: Selector
            // VideoResolution = new uint[] { 500, 500 };   // Graphics resolution
            // VideoResolution = new uint[] { 800, 600 };   // Graphics resolution
            // VideoResolution = new uint[] { 1366, 768 };  // Graphics resolution
            // VideoResolution = new uint[] { 1280, 720 };  // Graphics resolution
            // VideoResolution = new uint[] { 1920, 1080 }; // Graphics resolution

            _renderWindow = CreateRenderWindow(Styles.Default);
            LoadGround();

            // Load Player
            mainPlayer.Position = new Vector2f(_renderWindow.Size.X / 2, _renderWindow.Size.Y / 2);
            mainPlayer.TextureRect = playerTexture;
            mainPlayer.Scale = new Vector2f(3, 3);

            var texture = new Texture(GetRelativePath("Sprites\\Player\\Red\\redfront.png"));
            mainPlayer.Texture = texture;

            // Wall box
            _boxWall = LoadSprite("Sprites\\DesolatedHut.png", new IntRect(0, 0, 100, 100));
            _boxWall.Position = new Vector2f(250, 250);
            _boxWall.Scale = new Vector2f(0.5f, 0.5f);

            // Player postion from left, top (x, y)
            var coordText = new Text("", new Font(GetRelativePath("Fonts\\arial.ttf")));
            coordText.CharacterSize = 20;
            coordText.Position = new Vector2f(10, 10);

            while (_renderWindow.IsOpen)
            {
                _renderWindow.DispatchEvents(); // event handler to processes keystrokes/mouse movements
                _renderWindow.Clear();
                _renderWindow.Draw(_backgroundSprite);
                _renderWindow.Draw(_boxWall);
                _renderWindow.Draw(mainPlayer);

                // Print player coordinates left, top (x, y)
                coordText.DisplayedString = $"x {mainPlayer.Position.X} y {mainPlayer.Position.Y}";
                _renderWindow.Draw(coordText);

                if (_renderWindow.HasFocus()) // if window is focused
                {
                    InputControl();
                }

                _renderWindow.Display(); // update screen
            }
        }
        public void InputControl()
        {
                float movementSpeed = 5;
                float moveDistance = movementSpeed;
                float movementX = 0;
                float movementY = 0;

                if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                {
                    if (mainPlayer.CheckMovementCollision(0, -moveDistance, _boxWall))
                    {
                        //Console.WriteLine("Player collided with a wall");
                    }
                    else
                    {
                        movementY -= moveDistance;
                    }
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    if (mainPlayer.CheckMovementCollision(0, moveDistance, _boxWall))
                    {
                        //Console.WriteLine("Player collided with a wall");
                    }
                    else
                    {
                        movementY += moveDistance;
                    }
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    if (mainPlayer.CheckMovementCollision(moveDistance, 0, _boxWall))
                    {
                        //Console.WriteLine("Player collided with a wall");
                    }
                    else
                    {
                        movementX += moveDistance;
                    }

                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                {
                    if (mainPlayer.CheckMovementCollision(-moveDistance, 0, _boxWall))
                    {
                        //Console.WriteLine("Player collided with a wall");
                    }
                    else
                    {
                        movementX -= moveDistance;
                    }

                }
                mainPlayer.Translate(movementX, movementY); // move?
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

        private static Sprite LoadSprite(string path, IntRect square) // TODO: improve
        {
            var tmpSprite = new Sprite();

            var tmpRect = square;

            try
            {
                //tmpTexture
                var tmpTexture = new Texture(GetRelativePath(path)) { /*Repeated = false */};
                tmpSprite = new Sprite(tmpTexture, tmpRect);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Sprite load error:\n{e}");
            }

            return tmpSprite; // unsafe?
        }
        private static void LoadGround()
        {
            try
            {
                _backgroundTexture = new Texture(GetRelativePath("Sprites\\Ground\\Title_Image.png")) { Repeated = true };
                Console.WriteLine(GetRelativePath());
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
        private static string GetRelativePath(string myPath = "")
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
