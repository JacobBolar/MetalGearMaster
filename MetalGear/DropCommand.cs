using System;

namespace MetalGear
{
    //drop command
    public class DropCommand : Command
    {
        public DropCommand()
        {
            Name = "drop";
        }

        public override bool Execute(Snake snake)
        {
            if (HasSecondWord())
                snake.drop(SecondWord);
            else
                Console.WriteLine("\nDrop What?");

            return false;
        }
    }
}