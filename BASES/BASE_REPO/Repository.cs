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
    public class Repository
    {
        protected IDalSession DalSession;
        public int Timeout = GlobalApp.AppSettings.ConnectionInfo.Timeout;
        public Repository(IDalSession dalSession)
        {
            DalSession = dalSession;
        }

        public async Task<PagedResult<T>> GetAsync<T>(IQueryCollection keyValuePairs, int isDeleted = 0)
        {

            var dictionary = keyValuePairs.ToDictionary(x => x.Key, x => x.Value.ToString());
            var selectObject = dictionary.ToSelectQueryByKeyValues<T>(isDeleted);
            var countQuery = QueryBuilder.SelectCount<T>();
            var countRes = await DalSession.Connection().QueryFirstAsync<int>(countQuery.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);
            var result = await DalSession.Connection().QueryAsync<T>(selectObject.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);

            return new PagedResult<T>(result, countRes, dictionary.GetPageSize());
        }

        public async Task<T> GetAsync<T>(object id, int isDeleted = 0)
        {

            var selectObject = QueryBuilder.ToSelectQueryById<T>(id);
            var result = await DalSession.Connection().QueryAsync<T>(selectObject.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetListByQuery<T>(string query , dynamic dynamicObjects = null)
        {

            QueryParams selectObject = QueryBuilder.ToSelectQueryByQuery<T>(query,dynamicObjects);
            var result = await DalSession.Connection().QueryAsync<T>(selectObject.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);
            return result;
        }
        public async Task<T> GetByQuery<T>(string query, dynamic dynamicObjects = null)
        {

            QueryParams selectObject = QueryBuilder.ToSelectQueryByQuery<T>(query, dynamicObjects);
            var result = await DalSession.Connection().QueryFirstOrDefaultAsync<T>(selectObject.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);
            return result;
        }
        public async Task<IEnumerable<T>> GetListByCondition<T>(dynamic dynamicObjects, int page = 1, int pageSize = 10, bool getAll = false, int isDeleted = 0)
        {

            QueryParams selectObject = QueryBuilder.ToSelectQueryByObject<T>(dynamicObjects,page,pageSize,getAll,isDeleted);
            var result = await DalSession.Connection().QueryAsync<T>(selectObject.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);
            return result;
        }

        public async Task<T> GetByCondition<T>(dynamic dynamicObjects, int page = 1, int pageSize = 10, bool getAll = false, int isDeleted = 0)
        {

            QueryParams selectObject = QueryBuilder.ToSelectQueryByObject<T>(dynamicObjects, page, pageSize, getAll, isDeleted);
            var result = await DalSession.Connection().QueryAsync<T>(selectObject.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);
            return result.FirstOrDefault();
        }

        public async Task<I> InsertAsync<T,I>(T entity)
        {
            var insertQry = entity.InsertQuery();
            var result = await DalSession.Connection().QueryFirstAsync<dynamic>(insertQry.Query, insertQry.Parameters, DalSession.Tasnsaction(), Timeout);
            return (I)result.Res;
        }

        public async Task<T> UpdateAsync<T>(T entity)
        {
            var updateQry = entity.UpdateQuery();
            var result = await DalSession.Connection().QueryFirstAsync<T>(updateQry.Query, updateQry.Parameters, DalSession.Tasnsaction(), Timeout);
            return result;
        }

        public async Task<object> DeleteByIdAsync<T>(object id)
        {
            var deleteQry = QueryBuilder.DeleteByIdQuery<T>(id);
            var result = await DalSession.Connection().ExecuteAsync(deleteQry.Query, deleteQry.Parameters, DalSession.Tasnsaction(), Timeout);
            return id;
        }

        public async Task<int> DeleteByQueryAsync<T>(IQueryCollection keyValuePairs)
        {

            var selectObject = keyValuePairs.ToDictionary(x => x.Key, x => x.Value.ToString()).DeleteByQuery<T>();
            var result = await DalSession.Connection().ExecuteAsync(selectObject.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);
            return result;
        }
    }
}
