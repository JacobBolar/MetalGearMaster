namespace MetalGear
{
    //command used to buy items
    public class BuyCommand : Command 
    {
        public BuyCommand()
        {
            Name = "buy";
        }

        public override bool Execute(Snake snake)
        {
            if (HasSecondWord())
                snake.Buy(SecondWord);
            else
                snake.OutputMessage("\nBuy what?");
            return false;
        }
    }
}