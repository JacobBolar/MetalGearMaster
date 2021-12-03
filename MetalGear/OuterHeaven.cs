using System;
using System.Collections.Generic;

namespace MetalGear
{
    public class OuterHeaven
    {
        //singleton pattern
        private static OuterHeaven _instance;
        public static OuterHeaven Instance  
        {
            get
            {
                // if no instance create gamworld instance
                if (_instance == null) 
                {
                    // create gameworld
                    _instance = new OuterHeaven(); 
                }
                return _instance;
            }
        }
        //set entrance
        private Room _entrance;
        public Room Entrance 
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
        
        // transporter room
        private Room _transporter; 
        public Room Transporter
        {
            get { return _transporter; }
            set { _transporter = value; }
        }

        // trade room
        private Room _tradeRoom; 
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
        
        // room list to get random room
        private static List<Room> roomList = new(); 
        
        //OuterHeaven
        private OuterHeaven()
        {
            //Creating the world 
            CreateWorld();

            // subscribe to notification
            NotificationCenter.Instance.AddObserver("snakeEnteredRoom", enterRoom);
            NotificationCenter.Instance.AddObserver("snakePickedUpItem", pickUpItem);
            NotificationCenter.Instance.AddObserver("snakeDroppedItem", dropItem);
            NotificationCenter.Instance.AddObserver("snakeWentBack", back);
            NotificationCenter.Instance.AddObserver("snakeLeavingRoom", snakeLeavingRoom);
            NotificationCenter.Instance.AddObserver("snakeUnlockedDoor", unlockDoor);
            NotificationCenter.Instance.AddObserver("snakeGaveMasterKey", gaveMasterKey);
        }

        //Giving the masterkey to militant
        public void gaveMasterKey(Notification notification)
        {
            Snake snake = (Snake)notification.Object;
            
            //This line doesn't technically do anything, wanted to get militant to unlock the chest
            //but couldn't get it to work. Snake unlocks it instead
            //vvvvvvv
            Militant.Instance.ContainsMasterKey = true;
            snake.CurrentRoom.chest.unlock();
            Console.WriteLine("snake gave the master key to the militant");
        }
        

        //leaving room noti
        public void snakeLeavingRoom(Notification notification)
        {
            Snake snake = (Snake)notification.Object;
            Previous = snake.CurrentRoom;

        }

        //back notification
        public void back(Notification notification)
        {
            Snake snake = (Snake)notification.Object;
            Console.WriteLine("snake went back to previous room");
        }

        //drop noti
        public void dropItem(Notification notification)
        { 

            Console.WriteLine("snake dropped item");

        }
        
        //pickup notif
        public void pickUpItem(Notification notification) 
        {
            Console.WriteLine("snake picked up item");
            Snake snake = (Snake)notification.Object;
            if (snake.Inventory.ContainsKey("crown"))
            {
                Console.WriteLine("Congrats!! you beat the game!");
            }

        }
        
        //enter room notification
        public void enterRoom(Notification notification)
        {
            Snake snake = (Snake)notification.Object;
            //Transporter room.  If it lands on BigBossRoom, should randomise again.
            if(snake.CurrentRoom == Transporter)
            {
                Console.WriteLine("You have entered the transporter room");
                snake.CurrentRoom = RandomRoom();
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
        
        //unlock door notification
        public void unlockDoor(Notification notification)
        {
            Snake snake = (Snake)notification.Object;
            Console.WriteLine("snake has unlocked the Door");
        }
        
        //creating world function 
        private void CreateWorld() 
        {
            //creating all rooms
            Room entrancePlatform = new Room("on the main platform of Outer Heaven");
            Room researchRoom = new Room("Research and Development"); 
            Room armsRoom = new Room("the arms room.");
            Room barracksRoom = new Room("barracks room");
            Room medicalBay = new Room("medical bay");
            Room bigBossRoom = new Room("Big boss room");
            Room transportRoom = new Room("Transport Room");
            Room tradeRoom = new Room("Trade Room");
            
            //add all rooms to list for transporter 
            //not adding in transporter or bigbossroom
            roomList.Add(entrancePlatform);
            roomList.Add(researchRoom);
            roomList.Add(armsRoom);
            roomList.Add(barracksRoom);
            roomList.Add(medicalBay);
            roomList.Add(tradeRoom);

            //Delegate pattern for transport room.  Has to go after roomList or the
            //RandomRoom() function below will outofbounds
            RoomDelegate transportDelegate = new TransportRoom();
            transportDelegate.ContainingRoom = transportRoom;
            transportRoom.Delegate = transportDelegate;
            
            
            //create items/researchKey has researchTag for decorator design pattern
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

            //create chests and add items in chest
            ItemContainer researchChest = new ItemContainer("researchChest");
            researchKey.addDecorator(researchTag);
            researchChest.AddItem(researchKey);
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

            //setting variables to rooms
            Entrance = entrancePlatform;
            BigBossRoom = bigBossRoom;
            Transporter = transportRoom;
            TradeRoom = tradeRoom;
        }
        //Random Room method for tranport room/delegate in Room.cs
        public static Room RandomRoom()
        {
            Room tRoom = null;
            Random random = new Random();
            int r = random.Next(roomList.Count);
            tRoom = roomList[r];
            return tRoom;
        }
    }
}
