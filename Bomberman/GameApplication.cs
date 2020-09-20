using Microsoft.AspNetCore.SignalR.Client;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.IO;

namespace Bomberman
{
    class GameApplication
    {
        private static readonly GameApplication _instance = new GameApplication();

        RenderWindow window;
        Texture backgroundTexture;
        // TODO: Sprite arrays/lists to load
        Sprite backgroundSprite;
        Sprite Player;
        private uint[] videoResolution = { 800, 600 };

        private HubConnection _hubConnection;

        public static GameApplication GetInstance()
        {
            return _instance;
        }

        private void ConfigureHubConnection()
        {
            _hubConnection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:5001/user-hub")
                    .Build();

            _hubConnection.StartAsync().Wait();
        }

        public void Run()
        {
            string windowTitle = "Bomberman v0.01";
            // TODO: Selector
            // videoResolution = new uint[] { 500, 500 };   // Graphics resolution
            // videoResolution = new uint[] { 800, 600 };   // Graphics resolution
            // videoResolution = new uint[] { 1366, 768 };  // Graphics resolution
            // videoResolution = new uint[] { 1280, 720 };  // Graphics resolution
            // videoResolution = new uint[] { 1920, 1080 }; // Graphics resolution

            ConfigureHubConnection();

            window = CreateRenderWindow(Styles.Default, windowTitle, videoResolution);

            LoadGround();
            window.SetActive();

            // Fetching sprites
            // TODO: autonomic loading
            Player = LoadSprite("Sprites\\Player\\Red\\redfront.png", new IntRect(0, 0, 19, 32));
            Player.Position = new Vector2f((int)videoResolution[0] / 2, (int)videoResolution[1] / 2);
            Player.Scale = new Vector2f(3, 3);

            while (window.IsOpen)
            {
                window.DispatchEvents(); // event handler to processes keystrokes/mouse movements
                window.Clear();
                window.Draw(backgroundSprite);
                window.Draw(Player);
                window.Display(); // update screen

                InputControll();
            }
        }
        public void InputControll() // TODO: improve movement overlap, etc
        {
            Vector2f totalMovement = new Vector2f(0, 0);

            var tempPos = Player.Position;
            int speed = 4;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                totalMovement.Y -= speed;
                _hubConnection.InvokeAsync("SendMessage", "Asd", "asd").Wait();
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

            Player.Position = new Vector2f(tempPos.X + totalMovement.X, tempPos.Y + totalMovement.Y);
        }
        static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        public RenderWindow CreateRenderWindow(Styles windowStyle, string windowTitle, uint[] videoResolution)
        {
            VideoMode videoMode = new VideoMode(videoResolution[0], videoResolution[1]);
            RenderWindow window = new RenderWindow(videoMode, windowTitle, windowStyle);
            window.Closed += new EventHandler(OnClose); // close window

            window.SetMouseCursorVisible(true);
            window.SetFramerateLimit(60);

            Console.WriteLine("Resoultion: [ {0}x{1} ]", videoMode.Width, videoMode.Height);
            return window;
        }
        private Sprite LoadSprite(string path, IntRect square) // TODO: improve
        {
            Texture tmpTexture;
            Sprite tmpSprite = new Sprite();

            IntRect tmpRect = square;

            try
            {
                //tmpTexture
                tmpTexture = new Texture(getRelativePath(path)) { /*Repeated = false */};
                tmpSprite = new Sprite(tmpTexture, tmpRect);
            }
            catch (Exception e)
            {
                Console.WriteLine("Sprite load error:\n{0}", e.ToString());
            }

            return tmpSprite; // unsafe?
        }
        private void LoadGround()
        {
            try
            {
                backgroundTexture = new Texture(getRelativePath("Sprites\\Ground\\Title_Image.png")) { Repeated = true };
                Console.WriteLine(getRelativePath());
            }
            catch (Exception e)
            {
                Console.WriteLine("Sprite load error:\n{0}", e.ToString());
            }

            // left, top, width, length
            IntRect square = new IntRect(0, 0, (int)videoResolution[0], (int)videoResolution[1]);
            backgroundSprite = new Sprite(backgroundTexture, square);
        }
        /// <summary>
        /// Get relative path from executable dir ~\Bomberman\
        /// </summary>
        /// <param name="myPath"></param>
        /// <returns></returns>
        private string getRelativePath(string myPath = "")
        {
            string curPath, fullPath = "";
            string relPath = myPath;

            try
            {
                curPath = Directory.GetCurrentDirectory();
                fullPath = Path.GetFullPath(Path.Combine(curPath, @"..\..\..\", relPath));
            }
            catch (Exception e)
            {
                Console.WriteLine("Current directory load error: {0}", e.ToString());
            }

            return fullPath;
        }
    }
}
