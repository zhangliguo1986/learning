
using System.Reflection;

namespace Aop
{
    public class FoobarProxy<T> : DispatchProxy
    {
        public T Target { get; set; }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            Indicator.Injected = true;
            return targetMethod.Invoke(Target, args);
        }
    }
}