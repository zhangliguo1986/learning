
using System;
using System.Collections.Generic;
using System.Collections;
namespace ServiceProvider
{
    public interface IServiceLocator
    {
        void AddService<T>();

        T GetService<T>();
    }

    public class ServiceLocator : IServiceLocator
    {
        private readonly IDictionary<object, object> services;

        public ServiceLocator()
        {
            services = new Dictionary<object, object>();
        }

        public void AddService<T>()
        {
            services.TryAdd(typeof(T), Activator.CreateInstance<T>());
        }

        public T GetService<T>()
        {
            try
            {
                return (T)services[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("The requested service is not registered");
            }
        }
    }
}