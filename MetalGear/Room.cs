using System.Collections.Generic;

namespace MetalGear
{
    //Delegate design pattern
    public interface RoomDelegate
    {
        Door GetExit(string exitName);
        Room ContainingRoom { set; get; }
        Dictionary<string, Door> Exits { get; set; }
        string Description();
    }

    //making tranport room a delegate
    public class TransportRoom : RoomDelegate
    {
        private Room _containingRoom;
        private Door _rDoor;
        private Dictionary<string, Door> exits;

        public void SetExit(string exitName, Door door)
        {
            exits[exitName] = door;
        }

        public Door GetExit(string exitName)
        {
            return _rDoor;
        }

        public Room ContainingRoom
        {
            set
            {
                //Random function from outerheaven to generate a new room
                _containingRoom = value;
                _rDoor = new Door(_containingRoom, OuterHeaven.RandomRoom(), false);
            }
            get => _containingRoom;
        }

        public Dictionary<string, Door> Exits { get; set; }

        public string Description()
        {
            return "\n";
        }
    }

    //Room Class
    public class Room
    {
        private readonly Dictionary<string, Door> exits;
        public Dictionary<string, IItem> items;
        public ItemContainer chest; //new

        public string Tag { get; set; }

        private RoomDelegate _delegate;

        public RoomDelegate Delegate
        {
            set => _delegate = value;
        }

        public Room() : this("No Tag")
        {
        }

        public Room(string tag)
        {
            exits = new Dictionary<string, Door>();
            items = new Dictionary<string, IItem>();
            chest = new ItemContainer("chest");
            Tag = tag;
        }

        public void SetExit(string exitName, Door door)
        {
            exits[exitName] = door;
        }

        // get exit for current room
        public Door GetExit(string exitName)
        {
            Door door = null;
            exits.TryGetValue(exitName, out door);
            return door;
        }

        //get exits for the current room
        public string GetExits()
        {
            var exitNames = "Exits: ";
            var keys = exits.Keys;
            foreach (var exitName in keys) exitNames += " " + exitName;

            return exitNames;
        }

        public string Description()
        {
            return "You are in the " + Tag + ".\n *** " + GetExits();
        }

        //add item to room 
        public void addItem(ItemContainer itemContainer)
        {
            //items.Add(item.name, item);
            chest = itemContainer;
        }

        // display items in room 
        public string displayItems()
        {
            var returnString = "";
            foreach (var item in items.Keys) returnString += item + ", ";

            return chest.name;
        }
        
        // gets item from chest
        public IItem getItem(string item)
        {
            if (items.ContainsKey(item))
            {
                items.TryGetValue(item, out var I);
                return I;
            }

            return null;
        }
    }
}