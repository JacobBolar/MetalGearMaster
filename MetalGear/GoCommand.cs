namespace MetalGear
{
    //Used to move snake different directions
    public class GoCommand : Command
    {
        public GoCommand()
        {
            Name = "go";
        }

        override
            public bool Execute(Snake snake)
        {
            if (HasSecondWord())
                snake.WalkTo(SecondWord);
            else
                snake.OutputMessage("\nGo Where?");
            return false;
        }
    }
}