using System;

namespace MetalGear
{
    //Militant NPC using singleton.  Used as gate guard and BigBoss
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
                "\nMilitant: Bring Me the MasterKey.  You should know how to get it... go check the other four rooms.");
        }

        public void SpeakGive()
        {
            Console.WriteLine("\nMilitant: Looks like you made the masterKey.  Now GIVE me the masterKey. ");
        }

        public void SpeakChest()
        {
            Console.WriteLine(
                "\nMilitant: You gave me the key.  I have unlocked the chest for you.  INSPECT the chest, PICKUP the key, and UNLOCK the door north to proceed, " +
                "\nand DROP the masterKey back in the chest.  That key is too heavy for you to be carrying around.  Also, you may go to the trade room" +
                "\nnorth of the arms room and sell it there.  Feel free to buy it back if you want also.");
        }

        public void SpeakBigBoss()
        {
            Console.WriteLine(
                "\nBig Boss: Snake! You weren't supposed to make it here!  Whatever you do, don't INSPECT that chest and PICKUP whatever is inside!");
        }
    }
}