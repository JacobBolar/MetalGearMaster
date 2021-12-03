namespace MetalGear
{
    //Item interface
    public interface IItem
    {
        string name { get; set; }
        float weight { get; set; }
        string Description { get; }
        public bool grabbable { get; set; }
        void addDecorator(IItem decorator);
        int value { get; set; }
        bool isContainer { get; }
        void AddItem(IItem item);
        IItem RemoveItem(string itemName);
    }
}