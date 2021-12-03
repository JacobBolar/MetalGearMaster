using System;
using System.Collections.Generic;

namespace MetalGear
{
    public class Snake
    {
        //max weight snake can have
        private float maxWeight = 50;

        private int snakeValue;

        //inventory
        public Dictionary<string, IItem> Inventory { get; }

        //current room
        public Room CurrentRoom { get; set; }
        
        public Snake(Room room)
        {
            CurrentRoom = room;
            Inventory = new Dictionary<string, IItem>();
        }

        //snake walks to room
        public void WalkTo(string direction) 
        {
            //gets exit direction of door 
            var door = CurrentRoom.GetExit(direction); 

            if (door != null)
            {
                if (door.isLocked)
                {
                    Console.WriteLine("Door is locked");
                }
                else
                {
                    var snakeLeavingRoom = new Notification("snakeLeavingRoom", this);
                    // post notification to notification center
                    NotificationCenter.Instance
                        .PostNotification(snakeLeavingRoom); 
                    // assign other side of door to next room
                    var nextRoom = door.getOtherSideRoom(CurrentRoom);
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

        // back command 
        public void Back() 
        {
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
        //snake picksup item
        public void Pickup(string item) 
        {
            var i2 = CurrentRoom.chest.RemoveItem(item);
            if (i2 != null)
            {
                if (i2.grabbable)
                {
                    //checks if item can be picked up and is less than weight
                    if (maxWeight - i2.weight > 0) 
                    {
                        var notification = new Notification("snakePickedUpItem", this);
                        NotificationCenter.Instance.PostNotification(notification);
                        Inventory.Add(i2.name, i2);
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

        //snake drops item
        public void drop(string item) 
        {
            //check if item is in inventory 
            if (Inventory.ContainsKey(item)) 
            {
                IItem i;
                Inventory.TryGetValue(item, out i);
                Inventory.Remove(item);
                var notification = new Notification("snakeDroppedItem", this);
                NotificationCenter.Instance.PostNotification(notification);
                //return weight back to snake
                maxWeight = maxWeight + i.weight; 
                CurrentRoom.chest.AddItem(i);
            }
            else
            {
                Console.WriteLine("Item not in inventory");
            }
        }


        //unlocking door
        //the only door i have locked is the bigBossRoom
        public void Unlock(string direction) 
        {
            //gets exit direction door
            var door = CurrentRoom.GetExit(direction); 
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
        
        //inspect chest
        public void Inspect(string item) 
        {
            Console.WriteLine(CurrentRoom.chest.Description);
        }

        // sell item in trade room
        public void sell(string item) 
        {
            if (Inventory.ContainsKey(item) && CurrentRoom.Tag == "Trade Room")
            {
                IItem i;
                Inventory.TryGetValue(item, out i);
                //increase snakevalue
                snakeValue += i.value; 
                //remove from inventory
                Inventory.Remove(item); 
                //add item to room chest
                CurrentRoom.chest.AddItem(i); 
                // add weight back to inventory
                maxWeight += i.weight;
                Console.WriteLine("You sold " + item + " for " + i.value);
            }
            else
            {
                Console.WriteLine("Item not in inventory");
            }
        }

        //buy item in trade room
        public void Buy(string item) 
        {
            // checks if current room
            if (CurrentRoom.Tag == "Trade Room") 
            {
                //remove item from room
                var i2 = CurrentRoom.chest.RemoveItem(item); 
                if (i2 != null)
                {
                    //checks if item can be bought with snakes value and is less than weight
                    if (maxWeight - i2.weight > 0 &&
                        i2.value <= snakeValue) 
                    {
                        var notification = new Notification("snakePickedUpItem", this);
                        NotificationCenter.Instance.PostNotification(notification);
                        Inventory.Add(i2.name, i2);
                        //subtract from max weight
                        maxWeight = maxWeight - i2.weight; 
                        //adds value to snake
                        snakeValue -= i2.value; 
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

        // giving masterkey to militant
        public void Give(string item) 
        {
            if (Inventory.ContainsKey(item) && CurrentRoom.Tag == "on the main platform of Outer Heaven")
            {
                //militant gets masterKey
                Militant.Instance.ContainsMasterKey = true; 
                Militant.Instance.SpeakChest();
                var notification = new Notification("snakeGaveMasterKey", this);
                NotificationCenter.Instance.PostNotification(notification);
            }
            else
            {
                Console.WriteLine("Item cannot be given");
            }
        }

        //if stnake has all 4 keys and entersplatform, keys are dropped and masterkey is given.
        public void GiveMasterKey()
        {
            Pickup("masterKey");
        }
        
        //for game loop of checking for masterkey
        public void CheckforMasterKey()
        {
            //checking for all 4 keys and on entranceplatform
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

        //shows snake stats (basically just inventory and values
        public void Stats() 
        {
            Console.WriteLine("snake value:" + snakeValue + "\n" + "available inventory weight: " + maxWeight);
            var itemList = "Items: ";
            foreach (var i in Inventory.Values) itemList += "\n " + i.Description;
            Console.WriteLine(itemList);
        }
    }
}