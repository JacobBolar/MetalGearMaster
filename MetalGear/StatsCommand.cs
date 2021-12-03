namespace MetalGear
{
    public class StatsCommand : Command
    {
        public StatsCommand()
        {
            this.Name = "stats";
        }

        public override bool Execute(Snake snake)
        {
            snake.Stats();
            return false;
        }
    }
}