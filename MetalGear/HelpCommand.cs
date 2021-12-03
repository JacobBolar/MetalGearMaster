namespace MetalGear
{
    //Help Command
    public class HelpCommand : Command
    {
        private readonly CommandWords words;

        public HelpCommand() : this(new CommandWords())
        {
        }

        public HelpCommand(CommandWords commands)
        {
            words = commands;
            Name = "help";
        }

        override
            public bool Execute(Snake snake)
        {
            if (HasSecondWord())
                snake.OutputMessage("\nI cannot help you with " + SecondWord);
            else
                snake.OutputMessage("\nBig Boss: Snake..., \n\nHere are your options " + words.Description());
            return false;
        }
    }
}