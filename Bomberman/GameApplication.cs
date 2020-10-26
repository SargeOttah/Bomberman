﻿using Bomberman.Dto;
using Bomberman.Spawnables;
using Bomberman.Spawnables.Weapons;
using Bomberman.GUI;
using Bomberman.Spawnables.Obstacles;
using Bomberman.Global;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using Bomberman.Map;
using System.Runtime.CompilerServices;

namespace Bomberman
{
    internal class GameApplication
    {
        private static readonly GameApplication Instance = new GameApplication();

        private static RenderWindow _renderWindow;

        // TODO: Sprite arrays/lists to load
        private static Sprite _backgroundSprite;
        private static Sprite _boxWall;
        private static readonly uint[] VideoResolution = { 832, 576 };
        private const string WindowTitle = "Bomberman v0.02";

        //player init
        static Player mainPlayer = new Player();
        static List<Player> otherPlayers = new List<Player>();

        static IntRect playerTexture = new IntRect(0, 0, 19, 32);

        private static HubConnection _userHubConnection;

        // To track time
        Clock FrameClock { get; set; } = new Clock();
        Clock RespawnClock { get; set; } = new Clock();

        public static GameScore scoreBoard;

        public static TileMapFacade tileMapFacade;
        public static Texture mapSpriteSheet;


        public static GameApplication GetInstance()
        {
            return Instance;
        }

        private static void ConfigureHubConnections()
        {
            _userHubConnection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:5001/user-hub", (opts) =>
                    {
                        opts.HttpMessageHandlerFactory = (message) =>
                        {
                            if (message is HttpClientHandler clientHandler)
                            // bypass SSL certificate
                                clientHandler.ServerCertificateCustomValidationCallback +=
                                    (sender, certificate, chain, sslPolicyErrors) => { return true; };
                            return message;
                        };
                    })
                    .Build();

            _userHubConnection.On<PlayerDTO, string[]>("ClientConnected", ClientConnected); // Listens for our own PlayerDTO created by the server
            _userHubConnection.On("ReceiveMessage", (string user, string message) => Console.WriteLine($"{user}: {message}")); // Demo listener.
            
            // Receive bomb log
            _userHubConnection.On("ReceiveBombLocation", 
                (string user, PointF pos) => Console.WriteLine($"User: {user} Placed bomb at (x, y): [{pos.X}, {pos.Y}]"));
            // Receive bomb creation signal
            _userHubConnection.On<PointF>("ReceiveNewBomb", OnBombPlaced);


            _userHubConnection.On<PlayerDTO>("ReceiveNewClient", OnNewClientConnect); // Listens for new clients that connect to the server
            _userHubConnection.On<List<PlayerDTO>>("RefreshPlayers", RefreshPlayers); // Refreshes data for all players connected to the server ( currenty only position )
            _userHubConnection.StartAsync().Wait();
        }

        public void Run()
        {
            // Initializing the tilemap facade
            tileMapFacade = new TileMapFacade((int)VideoResolution[0], (int)VideoResolution[1], Properties.Resources.spritesheet2);

            ConfigureHubConnections();

            _renderWindow = CreateRenderWindow(Styles.Default);
            _renderWindow.SetFramerateLimit(60);
            _renderWindow.SetActive();

            
            // Wall box
            _boxWall = SpriteLoader.LoadSprite(Properties.Resources.DesolatedHut, new IntRect(0, 0, 100, 100));

            BoardBuilder board = new BoardBuilder();
            //Enemy enemy = SpawnEnemy("Zombie");


            // <Enemy> factory -> Enemy -> BoardBuilder -> Prototype implementation
            board.AddGhost(new Vector2f(200, 200), new Vector2f(0.2f, 0.2f));
            board.AddSkeleton(new Vector2f(100, 100), new Vector2f(2f, 2f));

            //Crate
            Sprite crate = SpawnObstacle();


            // Spawn obstacle
            //Sprite obs = SpawnObstacle();
            //obs.Position = new Vector2f(100, 100);

            _boxWall.Position = new Vector2f(250, 250);
            _boxWall.Scale = new Vector2f(0.5f, 0.5f);

            // UI score object
            scoreBoard = new GameScore(_renderWindow);

            // Player postion from left, top (x, y)
            var coordText = new Text("", new Font(Properties.Resources.arial));
            coordText.CharacterSize = 20;
            coordText.Position = new Vector2f(10, 10);

            // BombKey [SPACE] event handler
            _renderWindow.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

            // damage - placeDelay - bombTimer
            mainPlayer.Bomb = new Bomb(20, 500, 2000);

            while (_renderWindow.IsOpen)
            {
                Time deltaTime = FrameClock.Restart();

                // Requesting refresh data from server at every new frame
                _userHubConnection.InvokeAsync("RefreshPlayer", mainPlayer.GetPointPosition()).Wait();

                _renderWindow.DispatchEvents(); // event handler to processes keystrokes/mouse movements
                _renderWindow.Clear();

                // TILES
                _renderWindow.Draw(tileMapFacade.GetTileMap());
                
                _renderWindow.Draw(_boxWall);
                _renderWindow.Draw(mainPlayer);

                foreach (var p in board._enemies)
                {
                    _renderWindow.Draw(p.getSprite());
                }

                //_renderWindow.Draw(enemy.getSprite());
                //_renderWindow.Draw(obs);

                foreach (Player p in otherPlayers)
                {
                    _renderWindow.Draw(p);
                }

                _renderWindow.Draw(scoreBoard);

                //DEBUG - RED FRAME
                _renderWindow.Draw(mainPlayer.DrawFrame());

                // Update drawable destructor timers
                UpdateLoop(deltaTime);

                // BOMBS
                mainPlayer.Update();    // Update projectile placement positions (bombs)
                DrawLoop();             // Draw projectile buffer list

                if (mainPlayer.CheckCollisions()) // if collided with spawnables
                {
                    Time respawnTime = FrameClock.Restart();
                    float TimeSinceCreation = 0.0f;

                    while (TimeSinceCreation < 1.0f) // SLOWDOWN score polling
                    {
                        TimeSinceCreation += respawnTime.AsSeconds();
                        //Console.WriteLine("TimeSinceCreation {0}", TimeSinceCreation);
                    }
                    scoreBoard.UpdateScore("P1"); // update to check for player ID
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
                    //if (_serverBool)
                    //{
                    //    _userHubConnection.InvokeAsync("SendMessage", "Asd", "asd").Wait();
                    //}
                    // Demo sender - "SendMessage" maps to hub's function name.


                    if (mainPlayer.CheckMovementCollision(0, -moveDistance, tileMapFacade.GetTileMap().GetCloseObstacles(mainPlayer.Position)))
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
                    if (mainPlayer.CheckMovementCollision(0, moveDistance, tileMapFacade.GetTileMap().GetCloseObstacles(mainPlayer.Position)))
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
                    if (mainPlayer.CheckMovementCollision(moveDistance, 0, tileMapFacade.GetTileMap().GetCloseObstacles(mainPlayer.Position)))
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
                    if (mainPlayer.CheckMovementCollision(-moveDistance, 0, tileMapFacade.GetTileMap().GetCloseObstacles(mainPlayer.Position)))
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

        // [NOTE] Global OnKeyPressed event handler, not just bombs
        void OnKeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Space)
            {
                // Place BOMB for local player
                var target = mainPlayer.Position;
                mainPlayer.Bomb.PlaceBomb(target);

                // Send bomb signal for server players
                _userHubConnection.InvokeAsync("SendBombLocation", mainPlayer.connectionId, mainPlayer.GetPointPosition()).Wait();

                // Drawbacks: sync difference
                // Possible fix: handle ALL bomb creation on server
            }

            if (e.Code == Keyboard.Key.Z)
            {

            }
        }

        public void UpdateLoop(Time deltaTime) // Loop updating drawables / spawnables
        {
            UpdateBombs(deltaTime);
        }
        private void UpdateBombs(Time deltaTime)
        {
            mainPlayer.Bomb.UpdateSpawnables(deltaTime.AsSeconds());
        }
        
        public void DrawLoop()
        {
            // Draw Spawnables
            DrawSpawnables();

            // Add other drawables Non-destroyables etc.
        }
        private void DrawSpawnables()
        {
            mainPlayer.Bomb.DrawSpawnables(_renderWindow); // Draw bomb as spawnable
            mainPlayer.Bomb.DrawExplosions(_renderWindow); // Draw explosion spawnable after bomb
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

        // Called when this client connects to the server, receives the player information
        private static void ClientConnected(PlayerDTO playerDTO, string[] map)
        {
            Console.WriteLine("We have connected");
            Console.WriteLine(playerDTO.ToString());
            if (!tileMapFacade.SetupTileMap(map)) {
                Console.WriteLine("Invalid map");
            }
            mainPlayer = new Player(playerDTO);
        }

        // Called when client received signal of bomb creation "ReceiveNewBomb"
        private static void OnBombPlaced(PointF pos)
        {
            Console.WriteLine("Server bomb placed at: [{0}{1}]", pos.X, pos.Y);
            mainPlayer.Bomb.PlaceBomb(new Vector2f(pos.X, pos.Y));
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

            mainPlayer.UpdateStats(main);
            foreach (PlayerDTO pNew in others)
            {
                Player p = otherPlayers.Find(p => p.connectionId.Equals(pNew.connectionId));
                if (p != null)
                {
                    p.UpdateStats(pNew);
                } else
                {
                    otherPlayers.Add(new Player(pNew));
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

        //private Enemy SpawnEnemy(string name)
        //{
        //    EnemyFactory enemyFactory = new EnemyFactory();
        //    Enemy enemy = enemyFactory.createEnemy(name);
        //    enemy.Position(50, 100);
        //    enemy.Scale(0.2f, 0.2f);

        //    return enemy;
        //}
        private Sprite SpawnObstacle()
        {
            ObstacleFactory obsFactory = FactoryPicker.GetFactory("Destroyable");
            Sprite obj = obsFactory.GetDestroyable("Crate").SpawnObstacle();
            obj.Position = new Vector2f(100, 100);
            obj.Scale = new Vector2f(0.5f, 0.5f);

            return obj;
        }
    }
}
