namespace MetalGear
{
    public class QuitCommand : Command
    {
        public QuitCommand()
        {
            Name = "quit";
        }

        override
            public bool Execute(Snake snake)
        {
            var answer = true;
            if (HasSecondWord())
            {
                snake.OutputMessage("\nI cannot quit " + SecondWord);
                answer = false;
            }

            return answer;
        }
    }
}