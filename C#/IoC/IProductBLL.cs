using System;
using System.Collections;
using System.Collections.Generic;

namespace IoC
{
    public interface IProductBLL
    {
        IEnumerable<Product> GetProducts();

        IEnumerable<Product> GetProducts(string name);
    }
}