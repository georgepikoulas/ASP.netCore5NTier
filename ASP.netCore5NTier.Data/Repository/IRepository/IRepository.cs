using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ASP.netCore5NTier.Data.Repository.IRepository
{
    public interface IRepository <T> where T :  class
    {
        //method  for getting a single record
        T Find(int id);

        //method for getting more than one record . Also we can add optional parameters for Order, included tables(JOINs) , define if the query needs to be tracked or not. If the query is only for reading we can switch this off and get improved performance 
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true);
    }
}
