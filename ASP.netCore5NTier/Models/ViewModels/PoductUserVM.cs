using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.netCore5NTier.Models.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            Products = new List<Product>();
        }
        public ApplicationUser ApplicationUser { get; set; }
        public IList<Product> Products { get; set; }
    }
}
    