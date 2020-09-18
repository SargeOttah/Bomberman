using System;

namespace Bomberman
{
    class Program
    {
        static void Main(string[] args)
        {
            GameApplication game = GameApplication.GetInstance();

            Console.WriteLine("Starting game...");

            game.Run();

            Console.WriteLine("Game closed!");
        }
    }
}
