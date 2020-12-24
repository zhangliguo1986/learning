using System;

namespace DelegateAndEvent
{
    public class HRWithAddRemove
    {
        private SendDelegate sendDelegate;

        public void AddDelegate(SendDelegate sendDelegate)
        {
            this.sendDelegate += sendDelegate;
        }

        public void RemoveDelegate(SendDelegate sendDelegate)
        {
            this.sendDelegate -= sendDelegate;
        }

        public void SendMessage(string msg)
        {
            sendDelegate(msg);
        }
    }
}