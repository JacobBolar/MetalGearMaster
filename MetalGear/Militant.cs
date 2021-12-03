using System;

namespace MetalGear
{
    public class Militant
    {
        private static Militant _instance;

        public bool ContainsMasterKey { get; set; } = false;

        //Singleton Design Pattern
        public static Militant Instance
        {
            get
            {
                if (_instance == null) _instance = new Militant();
                return _instance;
            }
        }

        public void SpeakDeny()
        {
            Console.WriteLine(
                "Militant: Bring Me the MasterKey.  You should know how to get it... go check the other four rooms.");
        }

        public void SpeakGive()
        {
            Console.WriteLine("Militant: Looks like you made the masterKey.  Now GIVE me the masterKey. ");
        }

        public void SpeakChest()
        {
            Console.WriteLine(
                "Militant: You gave me the key.  I have unlocked the chest for you.  INSPECT the chest, PICKUP the key, and UNLOCK the door north to proceed, and DROP the keys back in the chest.  That key is too heavy for you to be carrying around.  Also, you may go to the trade room" +
                "north of the arms room and sell it there.  Feel free to buy it back if you want also.");
        }

        public void SpeakBigBoss()
        {
            Console.WriteLine(
                "Big Boss: Snake! You weren't supposed to make it here!  Whatever you do, don't INSPECT that chest and PICKUP whatever is inside!");
        }
    }
}