namespace MetalGear
{
    public class Door
    {
        private readonly Room room1;
        private readonly Room room2;

        //constructor with isLocked (strategy)
        public Door(Room room1, Room room2, bool isLocked)
        {
            this.room1 = room1;
            this.room2 = room2;
            this.isLocked = isLocked;
        }

        //check if door is locked
        //strategy design pattern
        public bool isLocked { get; set; }

        // assign other side of room
        public Room getOtherSideRoom(Room room)
        {
            if (room == room1)
                return room2;
            return room1;
        }

        //create doors and set exits
        //strategy design pattern
        public static Door CreateDoor(Room room1, Room room2, string direction1, string direction2, bool isLocked)
        {
            var door = new Door(room1, room2, isLocked);
            room1.SetExit(direction1, door);
            room2.SetExit(direction2, door);

            return door;
        }
    }
}