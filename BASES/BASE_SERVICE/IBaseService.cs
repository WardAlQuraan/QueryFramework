using BASES.BASE_REPO;
using CONNECTION_FACTORY.DAL_SESSION;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASES.BASE_SERVICE
{
    public interface IBaseService<T>
    {
        Task<IEnumerable<T>> GetAsync(IQueryCollection keyValuePairs);
        Task<T> GetAsync(object id);
        Task<object> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<object> DeleteByIdAsync(object id);
        Task<int> DeleteByQueryAsync(IQueryCollection keyValuePairs);
        
    }
}
