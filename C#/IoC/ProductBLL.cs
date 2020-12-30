using System;
using System.Collections.Generic;
using System.Linq;

namespace IoC
{
    public class ProductBLL : IProductBLL
    {
        private readonly IProductDAL _productDAL;

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