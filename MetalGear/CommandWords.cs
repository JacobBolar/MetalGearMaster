using System.Collections.Generic;

namespace MetalGear
{
    public class CommandWords
    {
        private readonly Dictionary<string, Command> commands;

        //NOTE BACK COMMAND IS NOT WORKING!!!!!!
        private static readonly Command[] commandArray =
        {
            new GoCommand(), new QuitCommand(), new BackCommand(), new PickupCommand(), new InspectCommand(),
            new StatsCommand(), new UnlockCommand(), new GiveCommand(), new DropCommand(), new BuyCommand(),
            new SellCommand()
        };

        public CommandWords() : this(commandArray)
        {
        }

        public CommandWords(Command[] commandList)
        {
            commands = new Dictionary<string, Command>();
            foreach (var command in commandList) commands[command.Name] = command;
            Command help = new HelpCommand(this);
            commands[help.Name] = help;
        }

        public Command Get(string word)
        {
            Command command = null;
            commands.TryGetValue(word, out command);
            return command;
        }

        public string Description()
        {
            var commandNames = "";
            var keys = commands.Keys;
            foreach (var commandName in keys) commandNames += " " + commandName;
            return commandNames;
        }
    }
}