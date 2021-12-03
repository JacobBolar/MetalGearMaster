using System;

namespace MetalGear
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
            game.Play();
            game.End();
        }
    }
}