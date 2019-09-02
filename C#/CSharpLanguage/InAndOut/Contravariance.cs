
using System;
namespace CSharpLanguage
{
    public interface IBar<in T>
    {
        void Print(T content);
    }

    public class Bar : IBar<object>
    {
        public void Print(object context)
        {
            Console.WriteLine(context);
        }
    }
}