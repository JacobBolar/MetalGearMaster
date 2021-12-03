using System;
using System.Collections.Generic;

namespace MetalGear
{
    //Notification Center for notification design pattern
    public class NotificationCenter
    {
        private readonly Dictionary<string, EventContainer> observers;
        private static NotificationCenter _instance;

        public static NotificationCenter Instance
        {
            get
            {
                if (_instance == null) _instance = new NotificationCenter();
                return _instance;
            }
        }

        public NotificationCenter()
        {
            observers = new Dictionary<string, EventContainer>();
        }

        private class EventContainer
        {
            private event Action<Notification> Observer;

            public void AddObserver(Action<Notification> observer)
            {
                Observer += observer;
            }

            public void RemoveObserver(Action<Notification> observer)
            {
                Observer -= observer;
            }

            public void SendNotification(Notification notification)
            {
                Observer(notification);
            }

            public bool IsEmpty()
            {
                return Observer == null;
            }
        }

        public void AddObserver(string notificationName, Action<Notification> observer)
        {
            if (!observers.ContainsKey(notificationName)) observers[notificationName] = new EventContainer();
            observers[notificationName].AddObserver(observer);
        }

        public void RemoveObserver(string notificationName, Action<Notification> observer)
        {
            if (observers.ContainsKey(notificationName))
            {
                observers[notificationName].RemoveObserver(observer);
                if (observers[notificationName].IsEmpty()) observers.Remove(notificationName);
            }
        }

        public void PostNotification(Notification notification)
        {
            if (observers.ContainsKey(notification.Name)) observers[notification.Name].SendNotification(notification);
        }
    }
}