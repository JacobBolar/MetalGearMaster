namespace MetalGear
{
    //used to go back to the previous room.
    public class BackCommand : Command
    {
        public BackCommand()
        {
            Name = "back";
        }

        public override bool Execute(Snake snake)
        {
            snake.Back();
            return false;
        }
    }
}