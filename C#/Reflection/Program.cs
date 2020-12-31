using System.Reflection;
using System;

namespace Reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GetTypeInfo();

            GetTypePropertyInfo();

            GetTypeConstructorInfo();

            GetTypeMethodInfo();
            // Console.ReadKey();
        }

        static void GetTypeInfo()
        {
            var type = typeof(Customer);

            Console.WriteLine($"Class：{type.Name}");
            Console.WriteLine($"Namespace: {type.AssemblyQualifiedName}");
            Console.WriteLine($"Assembly：{type.Assembly}");
            Console.WriteLine($"AssemblyQualifiedName：{type.AssemblyQualifiedName}");
            Console.WriteLine($"FullName:{type.FullName}");
            Console.WriteLine($"MemberType:{type.MemberType}");
        }

        static void GetTypePropertyInfo()
        {
            Type type = typeof(Customer);

            PropertyInfo[] propertyInfos = type.GetProperties();

            Console.WriteLine($"This list of properties of the Customer class are: --");

            foreach (PropertyInfo pInfo in propertyInfos)
            {
                Console.WriteLine($"{pInfo.Name}");
            }
        }

        static void GetTypeConstructorInfo()
        {
            var type = typeof(Customer);

            var ctorInfo = type.GetConstructors();
            Console.WriteLine($"The Customer class contains the following Constructors: --");
            foreach (ConstructorInfo c in ctorInfo)
            {
                Console.WriteLine($"{c}");
            }
        }

        static void GetTypeMethodInfo()
        {
            var type = typeof(Customer);
            MethodInfo[] methodInfo = type.GetMethods();
            Console.WriteLine($"The Methods of the Customer class are: ---");
            foreach (var m in methodInfo)
            {
                Console.WriteLine($"{m.Name}");
            }
        }
    }
}
