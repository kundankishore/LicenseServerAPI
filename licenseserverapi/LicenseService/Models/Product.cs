using System;
using System.Collections;
using System.Collections.Generic;

namespace LicenseService.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }


        public IList<SubProducts> SubProducts;
    }

    public class SubProducts
    {
        public Guid SubProductId { get; set; }
        public string Name { get; set; }
        public string ProductGroup { get; set; }
    }

}