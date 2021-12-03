namespace MetalGear
{
    public class UnlockCommand : Command
    {
        public UnlockCommand()
        {
            this.Name = "unlock";
        }

        public override bool Execute(Snake snake)
        {
            if (this.HasSecondWord())
            {
                snake.Unlock(this.SecondWord);
            }
            else
            {
                snake.OutputMessage("\nUnlock what?");
            }
            return false;
        }
    }
}