using System;

namespace MetalGear
{
    public class PickupCommand : Command
    {
        public PickupCommand()
        {
            this.Name = "pickup";
        }

        public override bool Execute(Snake snake)
        {
            if(this.HasSecondWord())
            {
                snake.Pickup(SecondWord);
            }
            else
            {
                Console.WriteLine("pickup what?");
            }

            return false;
        }
    }
}