using System;

namespace ServiceProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            Singleton singleton = Singleton.getInstance();
            System.Console.WriteLine("Singleton1 value1: " + Singleton.value1);
            System.Console.WriteLine("Singleton.value2: " + Singleton.value2);
        }
    }

    public class Singleton
    {
        private static Singleton singleton = new Singleton();
        public static int value1;
        public static int value2 = 0;
        
        private Singleton()
        {
            value1++;
            value2++;
        }

        public static Singleton getInstance()
        {
            return singleton;
        }
    }
}
