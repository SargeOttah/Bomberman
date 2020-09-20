using System;

namespace Bomberman
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Starting game...");

            var game = GameApplication.GetInstance();
            game.Run();

            Console.WriteLine("Game closed!");
        }
    }
}
