using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data.Repository.IRepository;
using ASP.netCore5NTier.Models;

namespace ASP.netCore5NTier.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDBContext _db;

        public CategoryRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category obj)
        {
            _db.Update(obj);
        }
    }
}
