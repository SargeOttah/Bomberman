using Bomberman.Command;
using Bomberman.Dto;
using Bomberman.Global;
using Bomberman.GUI;
using Bomberman.Map;
using Bomberman.Spawnables.Obstacles;
using Bomberman.Spawnables.Weapons;
using Microsoft.AspNetCore.SignalR.Client;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Bomberman.HubHandler;
using Bomberman.GUI.Visitor;
using Bomberman.GUI.Logger;

namespace Bomberman
{
    public class GameApplication
    {
        private static readonly GameApplication Instance = new GameApplication();

        private static RenderWindow _renderWindow;

        // TODO: Sprite arrays/lists to load
        private static Sprite _boxWall;
        private static readonly uint[] VideoResolution = { 832, 576 };
        private const string WindowTitle = "Bomberman v0.3";
        private BoardBuilder _boardBuilder;

        //player init
        public Player mainPlayer { get; set; } = new Player();
        public List<Player> otherPlayers { get; set; }  = new List<Player>();


        private static HubConnection _userHubConnection;
        private static readonly IUserHubClient UserHubClient = new UserHubClient();
        private static readonly IUserHubClient UserHubClientProxy = new UserHubClientProxy(UserHubClient);


        // To track time
        Clock FrameClock { get; set; } = new Clock();

        public GameScore scoreBoard;

        public TileMapFacade tileMapFacade { get; set; }

        private static IMovement buttonW, buttonS, buttonA, buttonD;

        private bool ghostDead = true;
        public DebugGUI _debugGui;
        Logger myLog;

        Logger chainLogger = new ErrorLogger();
        Logger chainLogger2 = new DebugLogger();
        Logger chainLogger3 = new DefaultLogger();

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
                                    (sender, certificate, chain, sslPolicyErrors) => true;
                            return message;
                        };
                    })
                    .Build();

            _userHubConnection.On<PlayerDTO, string[]>("ClientConnected", UserHubClientProxy.ClientConnected); // Listens for our own PlayerDTO created by the server
            _userHubConnection.On("ReceiveMessage", (string user, string message) => Console.WriteLine($"{user}: {message}")); // Demo listener.


            _userHubConnection.On<PlayerDTO>("ReceiveNewClient", UserHubClientProxy.OnNewClientConnect); // Listens for new clients that connect to the server
            _userHubConnection.On<List<PlayerDTO>>("RefreshPlayers", UserHubClientProxy.RefreshPlayers); // Refreshes data for all players connected to the server ( currently only position )
            
            // bombs
            _userHubConnection.On<BombDTO>("ReceiveNewBomb", UserHubClientProxy.OnNewBomb);
            _userHubConnection.On<BombExplosionDTO>("ReceiveNewExplosion", UserHubClientProxy.OnBombExplosion);
            _userHubConnection.On<string[]>("RefreshMap", UserHubClientProxy.RefreshMap);

            var enemiesCreated = false;
            _userHubConnection.On("RefreshEnemies", (string posX, string posY) =>
            {
                if (posX != null && posY != null)
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
                }
                else
                {
                    ghostDead = true;
                }
            });

            _userHubConnection.StartAsync().Wait();
        }

        public void Run()
        {
            // Initializing the tilemap facade
            tileMapFacade = new TileMapFacade((int)VideoResolution[0], (int)VideoResolution[1], Properties.Resources.spritesheet2);
            BindKeys();

            _boardBuilder = new BoardBuilder();
            ConfigureHubConnections();

            // Wall box
            _boxWall = SpriteLoader.LoadSprite(Properties.Resources.DesolatedHut, new IntRect(0, 0, 100, 100));

            _boxWall.Position = new Vector2f(250, 250);
            _boxWall.Scale = new Vector2f(0.5f, 0.5f);

            // UI score object
            _renderWindow = CreateRenderWindow(Styles.Default);
            _renderWindow.SetFramerateLimit(60);
            _renderWindow.SetActive();
            scoreBoard = new GameScore(_renderWindow, otherPlayers, mainPlayer.connectionId);

            _debugGui = new DebugGUI(_renderWindow);
            execRTCounter(); // sync with server

            // Player postion from left, top (x, y)
            var coordText = new Text("", new Font(Properties.Resources.arial));
            coordText.CharacterSize = 20;
            coordText.Position = new Vector2f(10, 10);

            // BombKey [SPACE] event handler
            _renderWindow.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

            // damage - placeDelay - bombTimer
            mainPlayer.Bomb = new Bomb(20, 2500, 0);

            // Chain object init
            

            // connecting chains
            chainLogger.setNextLogger(chainLogger2);
            chainLogger2.setNextLogger(chainLogger3);


            float RespawnPause = 0f;

            while (_renderWindow.IsOpen)
            {
                Time deltaTime = FrameClock.Restart();

                // Requesting refresh data from server at every new frame
                _userHubConnection.InvokeAsync("RefreshPlayer", new PlayerDTO(){ connectionId = mainPlayer.connectionId, position = mainPlayer.GetPointPosition(), IsDead = mainPlayer.IsDead}).Wait();

                _renderWindow.DispatchEvents(); // event handler to processes keystrokes/mouse movements
                _renderWindow.Clear();

                // TILES
                _renderWindow.Draw(tileMapFacade.GetTileMap());

                if (!ghostDead)
                {
                    foreach (var enemy in _boardBuilder._enemies)
                    {

                        var sprite = enemy.getSprite();
                        _renderWindow.Draw(sprite);
                    }
                }

                foreach (Player p in otherPlayers)
                {
                    if (!p.IsDead)
                    {
                        _renderWindow.Draw(p);
                    }
                }

                if (!mainPlayer.IsDead)
                {
                    _renderWindow.Draw(mainPlayer);

                    //DEBUG - RED FRAME
                    _renderWindow.Draw(mainPlayer.DrawFrame());
                }

                // Update drawable destructor timers
                UpdateLoop(deltaTime);

                // BOMBS
                mainPlayer.Update();    // Update projectile placement positions (bombs)
                DrawLoop();             // Draw projectile buffer list


                // RESPAWN ROUTINE
                /*if(RespawnPause > 0)
                    RespawnPause -= deltaTime.AsSeconds();*/

                /*if (mainPlayer.CheckDeathCollisions() && RespawnPause <= 0f) // if collided with flames?
                {
                    RespawnPause = 2f;
                    scoreBoard.UpdateScore("P1");
                }*/

                // Print player coordinates left, top (x, y)
                coordText.DisplayedString = $"x {mainPlayer.Position.X} y {mainPlayer.Position.Y}";
                _renderWindow.Draw(coordText);
                _renderWindow.Draw(scoreBoard);
                _renderWindow.Draw(_debugGui);

                if (_renderWindow.HasFocus()) // if window is focused
                {
                    InputControl();
                }

                _renderWindow.Display(); // update screen
            }
        }

        public void InputControl()
        { // invoker
            if (mainPlayer.IsDead) return;
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
                BombDTO bombDTO = mainPlayer.getBombDTO();
                _userHubConnection.InvokeAsync("OnBombPlace", bombDTO).Wait();
                chainLogger.logMessage(new Message(2, $"**\nPlayer [{mainPlayer.connectionId.Substring(0, 8)}] placed bomb\n**"));
            }

            if (e.Code == Keyboard.Key.Z)
            {
                mainPlayer.Bomb = new FastBomb();
                mainPlayer.Bomb.CurrentBombType = 2;

                Console.WriteLine("Bomb: [FastBomb]");
                Console.WriteLine("Damage: {0} PlaceSpeed: {1} BombTimer: {2}",
                    mainPlayer.Bomb.Damage, mainPlayer.Bomb.PlaceSpeed, mainPlayer.Bomb.IgnitionDuration);
            }
            if (e.Code == Keyboard.Key.X)
            {
                mainPlayer.Bomb = new SuperBomb();
                mainPlayer.Bomb.CurrentBombType = 1;


                Console.WriteLine("Bomb: [SuperBomb]");
                Console.WriteLine("Damage: {0} PlaceSpeed: {1} BombTimer: {2}",
                    mainPlayer.Bomb.Damage, mainPlayer.Bomb.PlaceSpeed, mainPlayer.Bomb.IgnitionDuration);
            }
            if (e.Code == Keyboard.Key.C)
            {
                mainPlayer.Bomb.CurrentBombType = 0;
                mainPlayer.Bomb = new Bomb(20, 2500, 0);

                Console.WriteLine("Bomb: [GeneralBomb]");
                Console.WriteLine("Damage: {0} PlaceSpeed: {1} BombTimer: {2}",
                    mainPlayer.Bomb.Damage, mainPlayer.Bomb.PlaceSpeed, mainPlayer.Bomb.IgnitionDuration);
            }
        }

        public void UpdateLoop(Time deltaTime) // Loop updating drawables / spawnables
        {
            UpdateBombs(deltaTime);
        }
        private void UpdateBombs(Time deltaTime)
        {
            mainPlayer.UpdateSpawnables(deltaTime.AsSeconds());
        }
        public void execRTCounter()
        {
            _debugGui.pVisitor.ResetData();

            foreach (Player p in otherPlayers)
            {
                if (!p.IsDead)
                {
                    p.accept(_debugGui.pVisitor); // player count
                }
            }
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

        private static void BindKeys()
        {
            buttonW = new MoveForward();
            buttonS = new MoveBackward();
            buttonA = new MoveLeft();
            buttonD = new MoveRight();

            //buttonZ = new MoveForward();
        }
    }
}
