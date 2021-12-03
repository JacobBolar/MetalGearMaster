using System;
namespace MetalGear
{
    public class SellCommand : Command // Command used to sell items in trading room
    {
        public SellCommand()
        {
            this.Name = "sell";
        }

        public override bool Execute(Snake snake)
        {
            if (this.HasSecondWord())
            {
                snake.sell(this.SecondWord);
            }
            else
            {
                snake.OutputMessage("\nSell what?");
            }
            return false;
        }
    }
}