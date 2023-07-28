using COMMON;
using COMMON.BUILDER;
using COMMON.HELPER;
using COMMON.PAGED;
using COMMON.QUERY_OBJECTS;
using CONNECTION_FACTORY.DAL_SESSION;
using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASES.BASE_REPO
{
    public class BaseRepo<T,I> 
    {
        protected IDalSession DalSession;
        public Repository Generic;
        public BaseRepo(IDalSession dalSession)
        {
            DalSession = dalSession;
            Generic = new Repository(DalSession);
        }

        public async Task<PagedResult<T>> GetAsync(IQueryCollection keyValuePairs , int isDeleted = 0) => await Generic.GetAsync<T>(keyValuePairs,isDeleted);
        public async Task<T> GetAsync(object id , int isDeleted = 0) => await Generic.GetAsync<T>(id,isDeleted);
        public async Task<IEnumerable<T>> GetListByCondition(dynamic dynamicObjects, int page = 1, int pageSize = 10, bool getAll = false, int isDeleted = 0) => await Generic.GetListByCondition<T>(dynamicObjects, page, pageSize, getAll, isDeleted);
        public async Task<T?> GetByCondition(dynamic dynamicObjects, int page = 1, int pageSize = 10, bool getAll = false, int isDeleted = 0) => await Generic.GetByCondition<T>(dynamicObjects,page, pageSize, getAll, isDeleted);
        public async Task<I> InsertAsync(T entity) => await Generic.InsertAsync<T,I>(entity);
        public async Task<T> UpdateAsync(T entity) => await Generic.UpdateAsync<T>(entity);
        public async Task<object> DeleteByIdAsync(object id) => await Generic.DeleteByIdAsync<T>(id);
        public async Task<int> DeleteByQueryAsync(IQueryCollection keyValuePairs) => await Generic.DeleteByQueryAsync<T>(keyValuePairs);
    }
}
