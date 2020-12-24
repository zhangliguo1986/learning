using System;
using System.Threading;

namespace DelegateAndEvent
{
    public class Sender
    {
        private readonly string senderName;

        public Sender(string name)
        {
            this.senderName = name;
        }

        public string Send(string message)
        {
            string serialNumber = Guid.NewGuid().ToString();
            Console.WriteLine(senderName + " sending....");
            Thread.Sleep(2000);
            Console.WriteLine("Sender: " + senderName + " , Content: " + message + ", Serial Number: " + serialNumber);
            return serialNumber;
        }

    }
}