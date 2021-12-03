namespace MetalGear
{
    //Stats command to look at snakes stats/inventory
    public class StatsCommand : Command
    {
        public StatsCommand()
        {
            Name = "stats";
        }

        public override bool Execute(Snake snake)
        {
            snake.Stats();
            return false;
        }
    }
}