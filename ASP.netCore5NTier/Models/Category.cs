using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.netCore5NTier.Models
{
    public class Category
    {
        [Key] 
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }   

    }
}
