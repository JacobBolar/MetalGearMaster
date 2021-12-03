using System;

namespace MetalGear
{
    public class Game
    {
        private readonly Snake snake;
        private readonly Parser parser;
        private bool playing;
        private Militant _militant;

        public Game()
        {
            playing = false;
            parser = new Parser(new CommandWords());
            snake = new Snake(OuterHeaven.Instance.Entrance);
            _militant = new Militant();
        }

        // Main Game Loop. Finished when self destruct device is picked up.
        public void Play()
        {
            //variable to break the loop
            bool finished = false;
            while (!finished)
            {
                Console.Write("\n>");
                //Game loop will always check for masterKey
                snake.CheckforMasterKey();
                var command = parser.ParseCommand(Console.ReadLine());
                if (command == null)
                {
                    Console.WriteLine("I don't understand...");
                }
                else
                {
                    finished = command.Execute(snake);
                    if (snake.Inventory.ContainsKey("selfDestructDevice")) finished = true;
                }
            }
        }

        public void Start()
        {
            playing = true;
            snake.OutputMessage(Welcome());
        }

        public void End()
        {
            playing = false;
            snake.OutputMessage(Goodbye());
        }

        public string Welcome()
        {
            return "This Game was inspired by the famous 1989 game, Metal Gear..." +
                   "\nBig Boss: Snake... This is Big Boss.  Your mission is to infiltrate " +
                   "the secret para-military Base 'Outer Heaver' and destroy their Metal Gear!" +
                   "\nSnake: Metal Gear? Remind me what that is again..." +
                   "\nBig Boss: Metal Gear is a bi-pedal tank capable of launching nuclear warheads!" +
                   "\nSnake: Copy all.  Snake out.." + snake.CurrentRoom.Description();
        }

        public string Goodbye()
        {
            return "\nBOOOOOOOOOM!!" +
                   "\nSnake takes the self destruct button and blows up Metal Gear with Big Boss inside!!" +
                   "\nSnake:  Looks like Big Boss wanted me to fail this mission so he could have his own secret Metal Gear... good thing I destroyed it! \n";
        }
    }
}