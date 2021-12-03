using System;

namespace MetalGear
{
    //Give Command, used for Giving masterKey to militant
    public class GiveCommand : Command
    {
        public GiveCommand()
        {
            Name = "give";
        }

        public override bool Execute(Snake snake)
        {
            if (HasSecondWord())
                snake.Give(SecondWord);
            else
                Console.WriteLine("Give What?");
            return false;
        }
    }
}