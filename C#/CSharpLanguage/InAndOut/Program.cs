using System;

namespace CSharpLanguage
{
    class Program
    {
        static void Main(string[] args)
        {

            //协变
            IFoo<string> fooStr = new Foo();
            IFoo<object> fooObj = fooStr;

            object memberName = fooObj.GetName();
            Console.WriteLine(memberName);
            Console.Read();


            //逆变
            IBar<object> barObj = new Bar();
            IBar<string> barStr = barObj;

            barStr.Print("Hello World");
            Console.Read();
        }
    }
}
