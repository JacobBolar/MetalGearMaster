using System;

namespace MetalGear
{
    public class PickupCommand : Command
    {
        public PickupCommand()
        {
            Name = "pickup";
        }

        public override bool Execute(Snake snake)
        {
            if (HasSecondWord())
                snake.Pickup(SecondWord);
            else
                Console.WriteLine("pickup what?");

            return false;
        }
    }
}