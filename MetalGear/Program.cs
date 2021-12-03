namespace MetalGear
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var game = new Game();
            game.Start();
            game.Play();
            game.End();
        }
    }
}