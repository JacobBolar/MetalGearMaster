namespace MetalGear
{
    //unlock command
    public class UnlockCommand : Command
    {
        public UnlockCommand()
        {
            Name = "unlock";
        }

        public override bool Execute(Snake snake)
        {
            if (HasSecondWord())
                snake.Unlock(SecondWord);
            else
                snake.OutputMessage("\nUnlock what?");
            return false;
        }
    }
}