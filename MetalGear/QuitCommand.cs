using System.Collections;
using System.Collections.Generic;

namespace MetalGear
{
    public class QuitCommand : Command
    {

        public QuitCommand() : base()
        {
            this.Name = "quit";
        }

        override
            public bool Execute(Snake snake)
        {
            bool answer = true;
            if (this.HasSecondWord())
            {
                snake.OutputMessage("\nI cannot quit " + this.SecondWord);
                answer = false;
            }
            return answer;
        }
    }
}