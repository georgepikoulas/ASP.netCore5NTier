using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data.Repository.IRepository;
using ASP.netCore5NTier.Models;

namespace ASP.netCore5NTier.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDBContext  _db;
        public ApplicationUserRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

    }
}
