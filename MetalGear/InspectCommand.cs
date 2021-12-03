namespace MetalGear
{
    //Inspect command, used for looking into chest
    public class InspectCommand : Command 
    {
        public InspectCommand()
        {
            Name = "inspect";
        }

        public override bool Execute(Snake snake)
        {
            if (HasSecondWord())
                snake.Inspect(SecondWord);
            else
                snake.OutputMessage("\nInspect what?");
            return false;
        }
    }
}