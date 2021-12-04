namespace MetalGear
{
    // this class will go back to the previous room
    public class BackCommand : Command
    {
        public BackCommand()
        {
            Name = "back";
        }

        override
            public bool Execute(Snake snake)
        {
            snake.Back();
            return false;
        }
    }
}