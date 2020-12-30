using System;

namespace IoC
{
    class Program
    {
        static void Main(string[] args)
        {
            // 创建容器实例
            var container = new Container();
            // 注册服务
            container.RegisterTransient<IProductDAL, ProductDAL>();
            container.RegisterTransient<IProductBLL, ProductBLL>();

            // 从容器中获取类型实例
            var productBLL = container.Resolve<IProductBLL>();
            var products = productBLL.GetProducts();

            foreach (var product in products)
            {
                Console.WriteLine(product.Name);
            }
            Console.ReadKey();

            // var methodInfo = typeof(Program).GetMethod("MethodA");
            // Console.WriteLine("Found method: {0}", methodInfo);
        }

        public void MethodA()
        {

        }
    }
}
