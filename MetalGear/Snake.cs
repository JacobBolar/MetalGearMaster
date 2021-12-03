using System;
using System.Collections.Generic;

namespace MetalGear
{
    public class Snake
    {
        private float maxWeight = 50;

        private int snakeValue;

        //private CareTaker careTaker;
        public Dictionary<string, IItem> Inventory { get; }

        public Room CurrentRoom { get; set; }


        public Snake(Room room)
        {
            CurrentRoom = room;
            Inventory = new Dictionary<string, IItem>();
            // careTaker = new CareTaker();
            // careTaker.add(new Momento(null));
        }

        public void WalkTo(string direction) //snake walks to room
        {
            var door = CurrentRoom.GetExit(direction); //gets exit direction door 

            if (door != null)
            {
                if (door.isLocked)
                {
                    Console.WriteLine("Door is locked");
                }
                else
                {
                    //careTaker.add(saveStateToMomento()); // add current room into caretaker class stack
                    var snakeLeavingRoom = new Notification("snakeLeavingRoom", this);
                    NotificationCenter.Instance
                        .PostNotification(snakeLeavingRoom); // post notification to notification center
                    var nextRoom = door.getOtherSideRoom(CurrentRoom); // assign other side of door to next room
                    CurrentRoom = door.getOtherSideRoom(CurrentRoom);
                    CurrentRoom = nextRoom;
                    var snakeEnteredRoom = new Notification("snakeEnteredRoom", this);
                    NotificationCenter.Instance.PostNotification(snakeEnteredRoom);
                    OutputMessage("\n" + CurrentRoom.Description());
                    Console.WriteLine("Items in room: " + CurrentRoom.displayItems());
                }
            }
            else
            {
                OutputMessage("\nThere is no door on " + direction);
            }
        }

        // public Momento saveStateToMomento() //creates new momento and saves current state/room
        // {
        //     return new Momento(CurrentRoom);
        // }

        // public void getStateFromMomento(Momento momento) //gets last "state"/room that was saved 
        // {
        //     try
        //     {
        //         CurrentRoom = momento.getState();
        //     }
        //     catch (NullReferenceException e)
        //     {
        //         Console.WriteLine("No previous room");
        //         
        //     }
        //
        //
        // }

        public void Back() // back command 
        {
            //getStateFromMomento(careTaker.Get());//get last state that was saved and assign to current room
            var room = CurrentRoom;
            if (room != null)
            {
                CurrentRoom = room;
                var notification = new Notification("snakeWentBack", this);
                NotificationCenter.Instance.PostNotification(notification);
                OutputMessage("\n" + CurrentRoom.Description());
                Console.WriteLine("Items in room: " + CurrentRoom.displayItems());
            }
            else
            {
                Console.WriteLine("No previous room");
            }
        }

        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }


        //Quick Notes on this method:
        //Using RemoveItem is the only way I could find to set the item to a variable
        //Because of this, in the else statements, I had to add the item back
        public void Pickup(string item) //snake picksup item
        {
            var i2 = CurrentRoom.chest.RemoveItem(item);
            if (i2 != null)
            {
                if (i2.grabbable == true)
                {
                    if (maxWeight - i2.weight > 0) //checks if item can be picked up and is less than weight
                    {
                        var notification = new Notification("snakePickedUpItem", this);
                        NotificationCenter.Instance.PostNotification(notification);
                        Inventory.Add(i2.name, i2);
                        //i2.addDecorator(i2);
                        maxWeight = maxWeight - i2.weight;
                        CurrentRoom.chest.RemoveItem(item);
                    }
                    else
                    {
                        OutputMessage("Item will exceed weight capacity.");
                        CurrentRoom.chest.AddItem(i2);
                    }
                }
                else
                {
                    Console.WriteLine("Item cannot be picked up");
                    CurrentRoom.chest.AddItem(i2);
                }
            }
            else
            {
                Console.WriteLine("Item not found");
            }
        }


        public void drop(string item) //snake drops item
        {
            if (Inventory.ContainsKey(item)) //check if item is in inventory 
            {
                IItem i;
                Inventory.TryGetValue(item, out i);
                Inventory.Remove(item);
                var notification = new Notification("snakeDroppedItem", this);
                NotificationCenter.Instance.PostNotification(notification);
                maxWeight = maxWeight + i.weight; //return weight back to snake
                CurrentRoom.chest.AddItem(i);
            }
            else
            {
                Console.WriteLine("Item not in inventory");
            }
        }
        
        
        public void Unlock(string direction) // tries to unlock door 
        {
            var door = CurrentRoom.GetExit(direction); //gets exit direction door
            var name = door.getOtherSideRoom(CurrentRoom).Tag;

            if (door.isLocked)
            {
                if (Inventory.ContainsKey("bigBossKey"))
                {
                    door.isLocked = false;
                    var notification = new Notification("snakeUnlockedDoor", this);
                    NotificationCenter.Instance.PostNotification(notification);
                }
                else
                {
                    Militant.Instance.SpeakDeny();
                }
            }
        }


        public void Inspect(string item) //inspect item
        {
            IItem I;
            Console.WriteLine(CurrentRoom.chest.Description);
        }

        public void sell(string item) // sell item in trade room
        {
            if (Inventory.ContainsKey(item) && CurrentRoom.Tag == "Trade Room")
            {
                IItem i;
                Inventory.TryGetValue(item, out i);
                snakeValue += i.value; //increase snakevalue
                Inventory.Remove(item); //remove from inventory
                CurrentRoom.chest.AddItem(i); //add item to room chest
                maxWeight += i.weight; // add weight back to inventory
                Console.WriteLine("You sold " + item + " for " + i.value);
            }
            else
            {
                Console.WriteLine("Item not in inventory");
            }
        }

        public void Buy(string item) //snake buys items from 
        {
            if (CurrentRoom.Tag == "Trade Room") // checks if current room
            {
                IItem a = null;
                var i2 = CurrentRoom.chest.RemoveItem(item); //remove item from room
                if (i2 != null)
                {
                    if (maxWeight - i2.weight > 0 &&
                        i2.value <= snakeValue) //checks if item can be picked up and is less than weight
                    {
                        var notification = new Notification("snakePickedUpItem", this);
                        NotificationCenter.Instance.PostNotification(notification);
                        Inventory.Add(i2.name, i2);
                        maxWeight = maxWeight - i2.weight; //subtract from max weight
                        snakeValue -= i2.value; //adds value to snake
                    }
                    else
                    {
                        CurrentRoom.chest.AddItem(i2);
                        Console.WriteLine("Item cannot be added to inventory");
                    }
                }
                else
                {
                    Console.WriteLine("Item not found");
                }
            }
            else
            {
                Console.WriteLine("Action cannot be performed");
            }
        }

        public void Give(string item) // give blueFlame to monster
        {
            if (Inventory.ContainsKey(item) && CurrentRoom.Tag == "on the main platform of Outer Heaven")
            {
                Militant.Instance.ContainsMasterKey = true; //monster has blueFlame
                Militant.Instance.SpeakChest();
                var notification = new Notification("snakeGaveMasterKey", this);
                NotificationCenter.Instance.PostNotification(notification);
            }
            else
            {
                Console.WriteLine("Item cannot be given");
            }
        }

        public void GiveMasterKey()
        {
            Pickup("masterKey");
        }

        public void GiveKeyRing()
        {
            Pickup("keyRing");
        }

        public void CheckforMasterKey()
        {
            if (Inventory.ContainsKey("researchKey") && Inventory.ContainsKey("barracksKey") &&
                Inventory.ContainsKey("armsKey") && Inventory.ContainsKey("medicalKey") &&
                CurrentRoom.Tag == "on the main platform of Outer Heaven")
            {
                OutputMessage("You have obtained all keys and made the Master Key!");
                GiveMasterKey();
                Militant.Instance.SpeakGive();
                Inventory.Remove("researchKey");
                Inventory.Remove("armsKey");
                Inventory.Remove("barracksKey");
                Inventory.Remove("medicalKey");
            }
        }

        public void Stats() //shows snake stats
        {
            Console.WriteLine("snake value:" + snakeValue + "\n" + "available inventory weight: " + maxWeight);

            var itemList = "Items: ";
            foreach (var i in Inventory.Values) itemList += "\n " + i.Description;

            Console.WriteLine(itemList);
        }
    }
}