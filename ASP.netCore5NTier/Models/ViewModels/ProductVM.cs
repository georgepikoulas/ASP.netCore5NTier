using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.netCore5NTier.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public  IEnumerable<SelectListItem> CategoryListItems { get; set; }
    }
}
