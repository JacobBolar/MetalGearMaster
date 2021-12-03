namespace MetalGear
{
    //Command Design Pattern
    public abstract class Command
    {
        public string Name { get; set; }

        public string SecondWord { get; set; }

        public Command()
        {
            Name = "";
            SecondWord = null;
        }

        public bool HasSecondWord()
        {
            return SecondWord != null;
        }

        public abstract bool Execute(Snake snake);
    }
}