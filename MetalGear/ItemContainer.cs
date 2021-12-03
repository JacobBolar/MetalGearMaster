using System.Collections.Generic;

namespace MetalGear
{
    //Hierarchy/Container Design Pattern
    public class ItemContainer : IItem
    {
        private readonly Dictionary<string, IItem> _chest;
        public string name { get; set; }
        private float _weight;

        public float weight
        {
            get
            {
                float containedWeight = 0;
                //Adding all weights together
                foreach (var item in _chest.Values) containedWeight += item.weight;
                return _weight + containedWeight;
            }
            set => _weight = value;
        }

        private string _description { get; set; }
        public bool grabbable { get; set; }

        public string Description
        {
            get
            {
                if (isLocked) return "Chest is locked";

                var itemList = "Items: ";
                foreach (var item in _chest.Values) itemList += "\n " + item.Description;
                return "Name: " + name + ", Weight: " + weight + "," + _description + "\n" + itemList;
            }
        }

        private IItem _decorator;
        public bool isContainer => true;
        public int value { get; set; }

        public bool isLocked { get; set; }

        public ItemContainer() : this("Chest")
        {
        }

        public ItemContainer(string name) : this(name, 1f)
        {
        }

        public ItemContainer(string name, float weight) : this(name, weight, true)
        {
        }

        public ItemContainer(string name, float weight, bool grab) : this(name, weight, grab, 0)
        {
        }

        public ItemContainer(string name, float weight, bool grab, int value)
        {
            this.name = name;
            _chest = new Dictionary<string, IItem>();
            this.weight = weight;
            grabbable = grab;
            this.value = value;
            _description = "Item Description: name: " + name + "," + "weight: " + weight + "," + "value: " + value +
                           "," + _decorator;
        }

        //Decorate design pattern
        public void addDecorator(IItem decorator) 
        {
            if (_decorator == null)
                _decorator = decorator;
            else
                _decorator.addDecorator(decorator);
        }

        //Add item to chest
        public void AddItem(IItem item)
        {
            _chest[item.name] = item;
        }

        //Remove item from chest
        public IItem RemoveItem(string itemName)
        {
            IItem item = null;
            _chest.Remove(itemName, out item);
            return item;
        }

        //Locks chest 
        public void Lock() //locks chest
        {
            isLocked = true;
        }

        //unlocks chest
        public void unlock() //unlocks chest
        {
            isLocked = false;
        }
    }
}