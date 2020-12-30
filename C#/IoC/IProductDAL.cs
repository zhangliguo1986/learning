using System;
using System.Collections;
using System.Collections.Generic;

namespace IoC
{
    public interface IProductDAL
    {
        IEnumerable<Product> GetProducts();

        IEnumerable<Product> GetProducts(string name);
    }
}