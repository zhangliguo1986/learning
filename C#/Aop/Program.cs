using System;
using System.Diagnostics;
using System.Reflection;

namespace Aop
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = DispatchProxy.Create<IFoobar, FoobarProxy<IFoobar>>();
            ((FoobarProxy<IFoobar>)proxy).Target = new Foobar();

            Debug.Assert(Indicator.Injected == false);
            Debug.Assert(proxy.Invoke() == 1);
            Debug.Assert(Indicator.Injected == true);
        }
    }
}
