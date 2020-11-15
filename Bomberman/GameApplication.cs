using Bomberman.Dto;
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
using System.Linq;
using Bomberman.Map;
using Bomberman.Command;
using System.Drawing;

namespace Bomberman
{
    internal class GameApplication
    {
        private static readonly GameApplication Instance = new GameApplication();

        private static RenderWindow _renderWindow;

        // TODO: Sprite arrays/lists to load
        private static Sprite _boxWall;
        private static readonly uint[] VideoResolution = { 832, 576 };
        private const string WindowTitle = "Bomberman v0.02";
        private BoardBuilder _boardBuilder;

        //player init
        static Player mainPlayer = new Player();
        static List<Player> otherPlayers = new List<Player>();

        
        private static HubConnection _userHubConnection;

        // To track time
        Clock FrameClock { get; set; } = new Clock();
        Clock RespawnClock { get; set; } = new Clock();

        public static GameScore scoreBoard;

        public static TileMapFacade tileMapFacade;

        // Input handler
        //InputHandler InputControl;

        private static IMovement buttonW, buttonS, buttonA, buttonD, buttonZ;

        public static GameApplication GetInstance()
        {
            return Instance;
        }

        private void ConfigureHubConnections()
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
            //_userHubConnection.On<PointF>("ReceiveNewBomb", OnBombPlaced);
            _userHubConnection.On<PointF>("ReceiveNewBomb", OnBombPlaced);


            _userHubConnection.On<PlayerDTO>("ReceiveNewClient", OnNewClientConnect); // Listens for new clients that connect to the server
            _userHubConnection.On<List<PlayerDTO>>("RefreshPlayers", RefreshPlayers); // Refreshes data for all players connected to the server ( currenty only position )
            
            var enemiesCreated = false;
            _userHubConnection.On("RefreshEnemies", (string posX, string posY) =>
            {
                if (!enemiesCreated)
                {
                    _boardBuilder.AddGhost(new Vector2f(int.Parse(posX), int.Parse(posY)), new Vector2f(0.2f, 0.2f));

                    enemiesCreated = true;
                }
                else
                {
                    _boardBuilder.MoveGhost(int.Parse(posX), int.Parse(posY));
                }
            });
            _userHubConnection.StartAsync().Wait();
        }

        public void Run()
        {
            // Initializing the tilemap facade
            tileMapFacade = new TileMapFacade((int)VideoResolution[0], (int)VideoResolution[1], Properties.Resources.spritesheet2);
            //InputControl = new InputHandler(mainPlayer, tileMapFacade);
            BindKeys();

            _boardBuilder = new BoardBuilder();
            _renderWindow = CreateRenderWindow(Styles.Default);
            _renderWindow.SetFramerateLimit(60);
            _renderWindow.SetActive();
            ConfigureHubConnections();

            // Wall box
            _boxWall = SpriteLoader.LoadSprite(Properties.Resources.DesolatedHut, new IntRect(0, 0, 100, 100));


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
            mainPlayer.Bomb = new Bomb(20, 600, 4000);

            while (_renderWindow.IsOpen)
            {
                Time deltaTime = FrameClock.Restart();

                // Requesting refresh data from server at every new frame
                _userHubConnection.InvokeAsync("RefreshPlayer", mainPlayer.GetPointPosition()).Wait();

                _renderWindow.DispatchEvents(); // event handler to processes keystrokes/mouse movements
                _renderWindow.Clear();

                // TILES
                _renderWindow.Draw(tileMapFacade.GetTileMap());
                foreach (var enemy in _boardBuilder._enemies)
                {
                    var sprite = enemy.getSprite();
                    _renderWindow.Draw(sprite);
                }

                foreach (Player p in otherPlayers)
                {
                    _renderWindow.Draw(p);
                }

                _renderWindow.Draw(mainPlayer);
                //DEBUG - RED FRAME
                _renderWindow.Draw(mainPlayer.DrawFrame());

                // Update drawable destructor timers
                UpdateLoop(deltaTime);

                // BOMBS
                mainPlayer.Update();    // Update projectile placement positions (bombs)
                DrawLoop();             // Draw projectile buffer list

                if (mainPlayer.CheckCollisions()) // if collided with spawnables
                {
                    //invoker.RouteDeath(); // Handles death and GUI
                    //mainPlayer.Position = mainPlayer.playerSpawn;

                    Time respawnTime = FrameClock.Restart();
                    float TimeSinceCreation = 0.0f;

                    while (TimeSinceCreation < 1.0f) // SLOWDOWN score polling
                    {
                        TimeSinceCreation += respawnTime.AsSeconds();
                        //Console.WriteLine("TimeSinceCreation {0}", TimeSinceCreation);
                    }
                    scoreBoard.UpdateScore("P1");
                }


                // Print player coordinates left, top (x, y)
                coordText.DisplayedString = $"x {mainPlayer.Position.X} y {mainPlayer.Position.Y}";
                _renderWindow.Draw(coordText);
                _renderWindow.Draw(scoreBoard);

                if (_renderWindow.HasFocus()) // if window is focused
                {
                    //InputControl.Control();
                    InputControl();
                }

                _renderWindow.Display(); // update screen
            }
        }

        public void InputControl()
        { // invoker
            float movementSpeed = 5;
            float moveDistance = movementSpeed;
            float movementX = 0;
            float movementY = 0;

            List<Obstacle> collidableObstacles = tileMapFacade.GetTileMap().GetCloseObstacles(mainPlayer.Position);

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                if (!mainPlayer.CheckMovementCollision(0, -moveDistance, collidableObstacles))
                {
                    buttonW.Execute(mainPlayer, -moveDistance);
                    //movementY -= moveDistance;
                }
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                if (!mainPlayer.CheckMovementCollision(0, moveDistance, collidableObstacles))
                {
                    buttonS.Execute(mainPlayer, moveDistance);
                    //movementY += moveDistance;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                if (!mainPlayer.CheckMovementCollision(moveDistance, 0, collidableObstacles))
                {
                    buttonD.Execute(mainPlayer, moveDistance);
                    //movementX += moveDistance;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                if (!mainPlayer.CheckMovementCollision(-moveDistance, 0, collidableObstacles))
                {
                    buttonA.Execute(mainPlayer, -moveDistance);
                    // movementX -= moveDistance;
                }
            }

            

            mainPlayer.Translate(movementX, movementY); // move?
        }

        // [NOTE] Global OnKeyPressed event handler, not just bombs
        void OnKeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var target = mainPlayer.Position;
            

            if (e.Code == Keyboard.Key.Space)
            {
                Console.WriteLine("point: {0}  vector: {1}", mainPlayer.GetPointPosition(), target);

                // Place BOMB for local player
                //bool bombSet = mainPlayer.Bomb.PlaceBomb(target);

                bool bombSet = mainPlayer.PlaceBomb(target);

                // Send bomb signal for server players
                if (bombSet)
                {
                    //(float dmg, float placeDelay, float bombTimer, Sprite projectileSprite, PointF pos)
                    //var tmp = new BombDTO(mainPlayer.Bomb.Damage, mainPlayer.Bomb.PlaceSpeed, mainPlayer.Bomb.BombTimer, 
                    //    mainPlayer.Bomb.ProjectileSprite, mainPlayer.GetPointPosition());
                    
                    // TODO: set bomb type with interface method, send location and type

                    //old
                     _userHubConnection.InvokeAsync("SendBombLocation", mainPlayer.connectionId, mainPlayer.GetPointPosition()).Wait();

                }

                // Drawbacks: sync difference
                // Possible fix: handle ALL bomb creation on server
            }

            if (e.Code == Keyboard.Key.Z)
            {
                mainPlayer.Bomb = new FastBomb();

                Console.WriteLine("Bomb: [FastBomb]");
                Console.WriteLine("Damage: {0} PlaceSpeed: {1} BombTimer: {2}",
                    mainPlayer.Bomb.Damage, mainPlayer.Bomb.PlaceSpeed, mainPlayer.Bomb.BombTimer);

                //bool bombSet = mainPlayer.Bomb.PlaceBomb(target);
                //if (bombSet)
                //{
                    //_userHubConnection.InvokeAsync("SendBombLocation", mainPlayer.connectionId, mainPlayer.GetVectorPosition()).Wait();
                //    //PlaceBridgeBomb(mainPlayer.Position);
                //}
            }
            if (e.Code == Keyboard.Key.X)
            {
                mainPlayer.Bomb = new SuperBomb();

                Console.WriteLine("Bomb: [SuperBomb]");
                Console.WriteLine("Damage: {0} PlaceSpeed: {1} BombTimer: {2}",
                    mainPlayer.Bomb.Damage, mainPlayer.Bomb.PlaceSpeed, mainPlayer.Bomb.BombTimer);

                //bool bombSet = mainPlayer.Bomb.PlaceBomb(target);
                //if (bombSet)
                //{
                //    _userHubConnection.InvokeAsync("SendBombLocation", mainPlayer.connectionId, mainPlayer.GetVectorPosition()).Wait();
                //    //PlaceBridgeBomb(mainPlayer.Position);

                //}
            }
            if (e.Code == Keyboard.Key.C)
            {
                mainPlayer.Bomb = new Bomb(20, 500, 2000);

                Console.WriteLine("Bomb: [GeneralBomb]");
                Console.WriteLine("Damage: {0} PlaceSpeed: {1} BombTimer: {2}",
                    mainPlayer.Bomb.Damage, mainPlayer.Bomb.PlaceSpeed, mainPlayer.Bomb.BombTimer);

                //bool bombSet = mainPlayer.Bomb.PlaceBomb(target);
                //if (bombSet)
                //{
                //    _userHubConnection.InvokeAsync("SendBombLocation", mainPlayer.connectionId, mainPlayer.GetVectorPosition()).Wait();
                //    //PlaceBridgeBomb(mainPlayer.Position);
                //}
            }
        }

        public void UpdateLoop(Time deltaTime) // Loop updating drawables / spawnables
        {
            UpdateBombs(deltaTime);
        }
        private void UpdateBombs(Time deltaTime)
        {
            mainPlayer.UpdateSpawnables(deltaTime.AsSeconds());
            //mainPlayer.Bomb.UpdateSpawnables(deltaTime.AsSeconds());
        }

        public void DrawLoop()
        {
            // Draw Spawnables
            DrawSpawnables();

            // Add other drawables Non-destroyables etc.
        }
        private void DrawSpawnables()
        {
            mainPlayer.DrawSpawnables(_renderWindow); // Draw bomb as spawnable
            mainPlayer.DrawExplosions(_renderWindow); // Draw explosion spawnable after bomb

            //mainPlayer.Bomb.DrawSpawnables(_renderWindow); // Draw bomb as spawnable
            //mainPlayer.Bomb.DrawExplosions(_renderWindow); // Draw explosion spawnable after bomb
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
            if (!tileMapFacade.SetupTileMap(map))
            {
                Console.WriteLine("Invalid map");
            }
            mainPlayer = new Player(playerDTO);
        }

        private static void BindKeys()
        {
            buttonW = new MoveForward();
            buttonS = new MoveBackward();
            buttonA = new MoveLeft();
            buttonD = new MoveRight();

            //buttonZ = new MoveForward();
        }
        private static void PlaceBridgeBomb(Vector2f pos)
        {
          //  mainPlayer.Bomb.PlaceBomb(pos);
        }

        // Called when client received signal of bomb creation "ReceiveNewBomb"
        private static void OnBombPlaced(PointF pos)
        {

            Console.WriteLine("Server bomb placed at: [{0}{1}]", pos.X, pos.Y);
            mainPlayer.PlaceBomb(new Vector2f(pos.X, pos.Y));
            //mainPlayer.Bomb.PlaceBomb(new Vector2f(pos.X, pos.Y));
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
                }
                else
                {
                    otherPlayers.Add(new Player(pNew));
                }
            }
        }

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
