using Bomberman.Dto;
using Microsoft.AspNetCore.SignalR.Client;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bomberman
{
    internal class GameApplication
    {
        private bool _serverBool = true;
        private static readonly GameApplication Instance = new GameApplication();

        private static RenderWindow _renderWindow;

        // TODO: Sprite arrays/lists to load
        private static Sprite _backgroundSprite;
        private static Sprite _boxWall;
        private static readonly uint[] VideoResolution = { 800, 600 };
        private const string WindowTitle = "Bomberman v0.01";

        //player init
        static Player mainPlayer = new Player();
        static List<Player> otherPlayers = new List<Player>();

        static IntRect playerTexture = new IntRect(0, 0, 19, 32);

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


            _userHubConnection.On<PlayerDTO>("ClientConnected", ClientConnected);

            _userHubConnection.StartAsync().Wait();

            _userHubConnection.On("ReceiveMessage", (string user, string message) => Console.WriteLine($"{user}: {message}")); // Demo listener.
            _userHubConnection.On<PlayerDTO>("ReceiveNewClient", OnNewClientConnect);
            _userHubConnection.On<List<PlayerDTO>>("RefreshPlayers", RefreshPlayers);

        }

        public void Run()
        {
            //if (_serverBool) { ConfigureHubConnections(); }
            ConfigureHubConnections();

            // TODO: Selector

            // VideoResolution = new uint[] { 800, 600 };   // Graphics resolution
            // VideoResolution = new uint[] { 1366, 768 };  // Graphics resolution
            // VideoResolution = new uint[] { 1280, 720 };  // Graphics resolution
            // VideoResolution = new uint[] { 1920, 1080 }; // Graphics resolution

            _renderWindow = CreateRenderWindow(Styles.Default);
            _renderWindow.SetFramerateLimit(60);
            LoadGround(Properties.Resources.Title_Image);
            _renderWindow.SetActive();

            // Wall box
            _boxWall = LoadSprite(Properties.Resources.DesolatedHut, new IntRect(0, 0, 100, 100));


            _boxWall.Position = new Vector2f(250, 250);
            _boxWall.Scale = new Vector2f(0.5f, 0.5f);

            // Player postion from left, top (x, y)
            var coordText = new Text("", new Font(Properties.Resources.arial));
            coordText.CharacterSize = 20;
            coordText.Position = new Vector2f(10, 10);

            while (_renderWindow.IsOpen)
            {
                _userHubConnection.InvokeAsync("Refresh", mainPlayer.GetPointPosition()).Wait();
                _renderWindow.DispatchEvents(); // event handler to processes keystrokes/mouse movements
                _renderWindow.Clear();
                _renderWindow.Draw(_backgroundSprite);
                _renderWindow.Draw(_boxWall);
                _renderWindow.Draw(mainPlayer);

                foreach (Player p in otherPlayers)
                {
                    _renderWindow.Draw(p);
                }

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
                    if (_serverBool) { 
                    _userHubConnection.InvokeAsync("SendMessage", "Asd", "asd").Wait();
                    // Demo sender - "SendMessage" maps to hub's function name.
                }

                if (mainPlayer.CheckMovementCollision(0, -moveDistance, _boxWall))
                    {
                        // Console.WriteLine("Player collided with a wall");
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
        private static Sprite LoadSprite(byte[] imageBitmap, IntRect square, bool repeated = false)
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
        private static void LoadGround(byte[] imageBitmap) // kindof useless method tbh
        {
            var square = new IntRect(0, 0, (int)VideoResolution[0], (int)VideoResolution[1]);
            Sprite backgroundSprite;
            try
            {
                backgroundSprite = LoadSprite(imageBitmap, square, true);
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"Sprite load error:\n{e}");
                backgroundSprite = LoadSprite(Properties.Resources.Title_Image, square, true); // loading a default background
            }

            // left, top, width, length
            
            _backgroundSprite = backgroundSprite;
        }
        // Called when this client connects to the server, receives the player information
        private static void ClientConnected(PlayerDTO playerDTO)
        {
            Console.WriteLine("we have connected");
            Console.WriteLine(playerDTO.ToString());
            mainPlayer = new Player(playerDTO);
        }
        // Called when a new client (except the current one) connects to the server, receives the other players information
        private static void OnNewClientConnect(PlayerDTO playerDTO)
        {
            Console.WriteLine("New client connected");
            Console.WriteLine(playerDTO.ToString());
            Player newPlayer = new Player(playerDTO);
            otherPlayers.Add(newPlayer);
        }

        private static void RefreshPlayers(List<PlayerDTO> players)
        {
            PlayerDTO main = players.Where(p => p.connectionId.Equals(mainPlayer.connectionId)).First();
            List<PlayerDTO> others = players.Where(p => !p.connectionId.Equals(mainPlayer.connectionId)).ToList();
            Console.WriteLine("refreshing players");

            mainPlayer.UpdateStats(main);
            foreach (PlayerDTO pNew in others) // galima ir geriau, bet kolkas del saugumo
            {
                foreach (Player p in otherPlayers)
                {
                    if (p.connectionId.Equals(pNew.connectionId))
                    {
                        p.UpdateStats(pNew);
                        break;
                    }
                }
            }
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
                Console.WriteLine(fullPath.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Current directory load error: {e}");
            }

            return fullPath;
        }
    }
}
