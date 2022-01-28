using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data.Repository.IRepository;
using ASP.netCore5NTier.Models;

namespace ASP.netCore5NTier.Data.Repository
{
    public class ApplicationTypeRepository : Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDBContext _db;
        public ApplicationTypeRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ApplicationType obj)
        {
            _db.ApplicationType.Update(obj);
        }
    }
}
