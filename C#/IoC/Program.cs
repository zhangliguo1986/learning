using System;

namespace IoC
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            container.RegisterTransient<IProductDAL, ProductDAL>();
            container.RegisterTransient<IProductBLL, ProductBLL>();

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
