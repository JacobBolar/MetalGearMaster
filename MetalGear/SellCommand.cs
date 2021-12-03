namespace MetalGear
{
    // Command used to sell items in trading room
    public class SellCommand : Command 
    {
        public SellCommand()
        {
            Name = "sell";
        }

        public override bool Execute(Snake snake)
        {
            if (HasSecondWord())
                snake.sell(SecondWord);
            else
                snake.OutputMessage("\nSell what?");
            return false;
        }
    }
}