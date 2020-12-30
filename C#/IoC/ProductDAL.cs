using System.Linq;
using System.Collections.Generic;
using System;

namespace IoC
{
    public class ProductDAL : IProductDAL
    {

        public readonly List<Product> _products;

        public ProductDAL()
        {
            _products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name= "iPhone 9",
                          Description = "iPhone 9 mobile phone" },
            new Product { Id = Guid.NewGuid(), Name= "iPhone X",
                          Description = "iPhone X mobile phone" }
        };
        }

        public IEnumerable<Product> GetProducts()
        {
            return _products;
        }

        public IEnumerable<Product> GetProducts(string name)
        {
            return _products.Where(p => p.Name.Contains(name))
            .ToList();
        }
    }
}