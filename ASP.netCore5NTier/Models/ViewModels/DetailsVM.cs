﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.netCore5NTier.Models.ViewModels
{
    public class ProductDetailsVM
    {
        public ProductDetailsVM()
        {
            Product = new Product();
        }
        public Product Product { get; set; }
        public bool  ExistsInCart { get; set; }
    }
}
