using System;
using log4net.Config;

namespace Bomberman
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Starting game...");

            XmlConfigurator.Configure();

            var game = GameApplication.GetInstance();
            game.Run();

            Console.WriteLine("Game closed!");
        }
    }
}
