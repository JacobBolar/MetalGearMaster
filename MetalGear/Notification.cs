using System.Collections.Generic;

namespace MetalGear
{
    //Notification Design Pattern
    public class Notification
    {
        public string Name { get; set; }
        public object Object { get; set; }
        public Dictionary<string, object> userInfo { get; set; }

        public Notification() : this("NotificationName")
        {
        }

        public Notification(string name) : this(name, null)
        {
        }

        public Notification(string name, object obj) : this(name, obj, null)
        {
        }

        public Notification(string name, object obj, Dictionary<string, object> userInfo)
        {
            Name = name;
            Object = obj;
            this.userInfo = userInfo;
        }
    }
}