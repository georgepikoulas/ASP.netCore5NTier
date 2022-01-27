using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.netCore5NTier.Models
{
    public class ApplicationType
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
       

    }
}
