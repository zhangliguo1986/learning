
using System;
using System.Collections;

namespace IoC
{
    /// <summary>
    /// IOC 容器累
    /// </summary>
    public class Container
    {
        /// <summary>
        /// 容器类型存储
        /// </summary>
        private readonly Hashtable _registrations;

        public Container()
        {
            _registrations = new Hashtable();
        }
        /// <summary>
        /// 注册类型到容器中
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        public void RegisterTransient<TInterface, TImplementation>()
        {
            _registrations.Add(typeof(TInterface), typeof(TImplementation));
        }

        /// <summary>
        /// 创建类型（在容器中注册的类型）实例
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public TInterface Create<TInterface>()
        {

            var typeofImpl = (Type)_registrations[typeof(TInterface)];
            if (typeofImpl == null)
            {
                throw new ApplicationException($"Failed to resolve {typeof(TInterface).Name}");
            }
            return (TInterface)Activator.CreateInstance(typeofImpl);
        }

        public T Resolve<T>()
        {
            // 获取类型构造函数
            var ctor = ((Type)_registrations[typeof(T)]).GetConstructors()[0];
            // 获取类型构造函数参数
            var parameterType = ctor.GetParameters()[0].ParameterType;
            // 返回：表示具有指定名称的公共方法的对象（如果找到的话）；否则为 null
            var methodInfo = typeof(Container).GetMethod("Create");

            // parameterType: 要替换当前泛型方法定义的类型参数的类型数组。
            // 返回：表示通过将当前泛型方法定义的类型参数替换为 typeArguments 的元素生成的构造方法。
            var gm = methodInfo.MakeGenericMethod(parameterType);
            return (T)ctor.Invoke(new object[] { gm.Invoke(this, null) });

        }

    }
}