using System;
using System.Collections.Generic;

namespace MetalGear
{
    public class OuterHeaven
    {
        private static OuterHeaven _instance;
        public static OuterHeaven Instance  //singleton pattern
        {
            get
            {

                if (_instance == null) // if no instance create gamworld instance
                {
                    _instance = new OuterHeaven(); // create gameworld
                }
                return _instance;
            }
        }
        
        private Room _entrance;
        public Room Entrance //set entrance
        {
            get
            {
                return _entrance;
            }

            private set { _entrance = value; }
        }
        
        private Room Previous;
        public Room _previousRoom
        {
            get { return Previous; }
            private set { Previous = value; }
        }
        
        private Room _transporter; // transporter room
        public Room Transporter
        {
            get { return _transporter; }
            set { _transporter = value; }
        }

        private Room _tradeRoom; // trade room
        public Room TradeRoom
        {
            get { return _tradeRoom; }
            set { _tradeRoom = value; }
        }

        private static Room _bigBossRoom;
        public static Room BigBossRoom
        {
            get { return _bigBossRoom; }
            private set { _bigBossRoom = value; }
        }
        
        private static List<Room> roomList = new(); // room list to get random room


        private OuterHeaven()
        {
            CreateWorld(); // creates world

            // subscribe to notification
            NotificationCenter.Instance.AddObserver("snakeEnteredRoom", enterRoom); // subscribe to notification
            NotificationCenter.Instance.AddObserver("snakePickedUpItem", pickUpItem);
            NotificationCenter.Instance.AddObserver("snakeDroppedItem", dropItem);
            NotificationCenter.Instance.AddObserver("snakeWentBack", back);
            NotificationCenter.Instance.AddObserver("snakeLeavingRoom", snakeLeavingRoom);
            // NotificationCenter.Instance.AddObserver("snakeBrokeIce",snakeBrokeIce);
            NotificationCenter.Instance.AddObserver("snakeUnlockedDoor", unlockDoor);
            NotificationCenter.Instance.AddObserver("snakeGaveMasterKey", gaveMasterKey);

        }

        public void gaveMasterKey(Notification notification)
        {
            Snake snake = (Snake)notification.Object;
            Militant.Instance.ContainsMasterKey = true; //monster has blueFlame
            snake.CurrentRoom.chest.unlock();
            Console.WriteLine("snake gave the master key to the militant");
        }
        


        public void snakeLeavingRoom(Notification notification) //snake leaving room notification
        {
            Snake snake = (Snake)notification.Object;
            Previous = snake.CurrentRoom;

        }

        public void back(Notification notification) // snake went back notification 
        {
            Snake snake = (Snake)notification.Object;
            Console.WriteLine("snake went back to previous room");
        }


        public void dropItem(Notification notification) //drop item notification
        { 

            Console.WriteLine("snake dropped item");

        }
        public void pickUpItem(Notification notification) // pick up notification
        {
            Console.WriteLine("snake picked up item");
            Snake snake = (Snake)notification.Object;
            if (snake.Inventory.ContainsKey("crown"))
            {
                Console.WriteLine("Congrats!! you beat the game!");
            }

        }
        public void enterRoom(Notification notification) // enter room notification 
        {
            Snake snake = (Snake)notification.Object;
            //Transporter room.  If it lands on BigBossRoom, should randomise again.
            if(snake.CurrentRoom == Transporter)
            {
                Console.WriteLine("You have entered the transporter room");
                // Random random = new Random();
                // int r = random.Next(roomList.Count); 
                snake.CurrentRoom = RandomRoom();
                // while (snake.CurrentRoom == BigBossRoom)
                // {
                //     int a = random.Next(roomList.Count); 
                //     snake.CurrentRoom = roomList[r];
                // }
            }
            if(snake.CurrentRoom == TradeRoom)
            {
                Console.WriteLine("You may buy and sell items here");
            }
            if (snake.CurrentRoom == _bigBossRoom)
            {
                Militant.Instance.SpeakBigBoss();
            }
        }

        
        
      
        public void unlockDoor(Notification notification) // unlock door notification
        {
            Snake snake = (Snake)notification.Object;
            Console.WriteLine("snake has unlocked the Door");
        }
        
        private void CreateWorld() // create rooms , set exits, create items
        {
            Room entrancePlatform = new Room("on the main platform of Outer Heaven");
            Room researchRoom = new Room("Research and Development"); 
            Room armsRoom = new Room("the arms room.");
            Room barracksRoom = new Room("barracks room");
            Room medicalBay = new Room("medical bay");
            Room bigBossRoom = new Room("Big boss room");
            Room transportRoom = new Room("Transport Room");
            Room tradeRoom = new Room("Trade Room");
            
            //add all rooms to list for transporter 
            roomList.Add(entrancePlatform);
            roomList.Add(researchRoom);
            roomList.Add(armsRoom);
            roomList.Add(barracksRoom);
            roomList.Add(medicalBay);
            roomList.Add(tradeRoom);

            RoomDelegate transportDelegate = new TransportRoom();
            transportDelegate.ContainingRoom = transportRoom;
            transportRoom.Delegate = transportDelegate;
            
            
            //create items
            IItem researchKey = new Item("researchKey", 2, true, 50);
            IItem researchTag = new Item("researchTag", 1, true, 10);
            Item armsKey = new Item("armsKey", 2, true,8);
            Item barracksKey = new Item("barracksKey", 3, true,10);
            Item medicalKey = new Item("medicalKey", 1, true,2);
            Item masterKey = new Item("masterKey", 30, true, 100);
            Item bigBossKey = new Item("bigBossKey", 2, true, 500);
            Item selfDestructDevice = new Item("selfDestructDevice", 10, true, 1000);
            Item researchTube = new Item("researchTube", 10, false, 1000);
            Item rocketLauncher = new Item("rocketLauncher", 100, true, 1000);
            Item keyRing = new Item("keyRing", 1, true, 100);
            IItem dec1orator = new Item("decoratorTest,", 3, true, 400);
            
            //create chests and add items in chest
            ItemContainer researchChest = new ItemContainer("researchChest");
            researchKey.addDecorator(researchTag);
            
            researchChest.AddItem(researchKey);
            Console.WriteLine(researchTag.Description);
            Console.WriteLine(researchKey.Description);
            Console.WriteLine("TESTETSET");
            researchChest.AddItem(researchTube);
            researchRoom.addItem(researchChest);

            ItemContainer armsChest = new ItemContainer("armsChest");
            armsChest.AddItem(armsKey);
            armsChest.AddItem(rocketLauncher);
            armsRoom.addItem(armsChest);

            ItemContainer barracksChest = new ItemContainer("barracksChest");
            barracksChest.AddItem(barracksKey);
            barracksRoom.addItem(barracksChest);

            ItemContainer medicalChest = new ItemContainer("medicalChest");
            medicalChest.AddItem(medicalKey);
            medicalBay.addItem(medicalChest);

            ItemContainer entranceChest = new ItemContainer("entranceChest");
            entranceChest.AddItem(masterKey);
            entranceChest.AddItem(bigBossKey);
            entranceChest.AddItem(keyRing);
            entranceChest.isLocked = true;
            entrancePlatform.addItem(entranceChest);

            ItemContainer bigBossChest = new ItemContainer("bigBossChest");
            bigBossChest.AddItem(selfDestructDevice);
            bigBossRoom.addItem(bigBossChest);
            
            
            // create doors and sets exits for each room
            Door door = Door.CreateDoor(entrancePlatform, bigBossRoom, "north", "south",true);
            door = Door.CreateDoor(entrancePlatform, transportRoom, "south", "north",false);
            door = Door.CreateDoor(entrancePlatform, researchRoom, "west", "east",false);
            door = Door.CreateDoor(entrancePlatform, barracksRoom, "east", "west",false);
            door = Door.CreateDoor(researchRoom, medicalBay, "north", "south",false);
            door = Door.CreateDoor(barracksRoom, armsRoom, "north", "south",false);
            door = Door.CreateDoor(armsRoom, tradeRoom, "north", "south",false);

            Entrance = entrancePlatform;
            BigBossRoom = bigBossRoom;
            Transporter = transportRoom; //transporter room
            TradeRoom = tradeRoom;
        }
        
        public static Room RandomRoom()
        {
            Room tRoom = null;
            Random random = new Random();
            int r = random.Next(roomList.Count);
            Console.WriteLine(r);
            tRoom = roomList[r];
            Console.WriteLine(tRoom);
            return tRoom;
        }
    }
}
