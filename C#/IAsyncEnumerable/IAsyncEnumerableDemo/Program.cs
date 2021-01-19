using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IAsyncEnumerableDemo
{
    class Program
    {
        const int DELAY = 1000;
        const int MIN = 1;
        const int MAX = 10;

        static async Task Main(string[] args)
        {
            await foreach (var number in GetData())
            {
                Console.WriteLine($"{DateTime.Now}: number={number}");
            }
            
            Console.ReadKey();
        }

        static async IAsyncEnumerable<int> GetData()
        {
            for (int i = MIN; i < MAX; i++)
            {
                yield return i;
                await Task.Delay(DELAY);
            }
        }
    }
}
