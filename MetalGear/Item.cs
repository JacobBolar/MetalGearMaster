namespace MetalGear
{
    //Item classes, IItem interface
    public class Item : IItem
    {
        public string name { get; set; }
        private float _weight;

        public float weight
        {
            get
            {
                if (_decorator == null)
                    return _weight;
                return _weight + _decorator.weight;
            }
            set => _weight = value;
        }

        private string _description { get; set; }

        //Grabbable is if it can be picked up or not
        public bool grabbable { get; set; }
        public string Description => _description;
        private IItem _decorator;
        public bool isContainer => false;
        public int value { get; set; }

        public Item() : this("NoName")
        {
        }

        public Item(string name) : this(name, 1f)
        {
        }

        public Item(string name, float weight) : this(name, weight, true)
        {
        }

        public Item(string name, float weight, bool grab) : this(name, weight, grab, 0)
        {
        }

        public Item(string name, float weight, bool grab, int value)
        {
            this.name = name;
            this.weight = weight;
            grabbable = grab;
            this.value = value;
            _description = "Item Description: name: " + name + "," + "weight: " + weight + "," + "value: " + value +
                           "," + _decorator;
            _decorator = null;
        }

        public void addDecorator(IItem decorator)
        {
            if (_decorator == null)
                _decorator = decorator;
            else
                _decorator.addDecorator(decorator);

            UpdateDescription();
        }

        public void UpdateDescription()
        {
            _description = "Item Description: name: " + name + "," + "weight: " + weight + "," + "value: " + value +
                           "," + _decorator.name;
        }

        public void AddItem(IItem item)
        {
        }

        public IItem RemoveItem(string itemName)
        {
            return null;
        }
    }
}