using System;
using System.Collections.Generic;
using System.Linq;

namespace IoC
{
    public class ProductBLL : IProductBLL
    {
        private readonly IProductDAL _productDAL;

        /// <summary>
        /// 依赖注入：高层ProductBLL对底层ProductDAL的依赖，通过ProductBLL(IProductDAL productDAL)构造函数注入到ProductBLL中
        /// </summary>
        /// <param name="productDAL"></param>
        public ProductBLL(IProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productDAL.GetProducts();
        }

        public IEnumerable<Product> GetProducts(string name)
        {
            return _productDAL.GetProducts(name);
        }
    }
}