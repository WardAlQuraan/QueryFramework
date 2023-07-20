using COMMON;
using COMMON.BUILDER;
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
    public class BaseRepo<T> 
    {
        protected IDalSession DalSession;
        public int Timeout = GlobalApp.AppSettings.ConnectionInfo.Timeout;
        public BaseRepo(IDalSession dalSession)
        {
            DalSession = dalSession;
        }

        public async Task<IEnumerable<T>> GetAsync(IQueryCollection keyValuePairs)
        {

            var selectObject = keyValuePairs.ToDictionary(x => x.Key, x => x.Value.ToString()).ToSelectQueryByKeyValues<T>();
            var result = await DalSession.Connection().QueryAsync<T>(selectObject.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);
            return result;
        }

        public async Task<T> GetAsync(object id)
        {

            var selectObject = QueryBuilder.ToSelectQueryById<T>(id);
            var result = await DalSession.Connection().QueryFirstAsync<T>(selectObject.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);
            return result;
        }

        public async Task<object> InsertAsync(T entity)
        {
            var insertQry = entity.InsertQuery();
            var result = await DalSession.Connection().QueryFirstAsync<int>(insertQry.Query, insertQry.Parameters, DalSession.Tasnsaction(), Timeout);
            return result;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var updateQry = entity.UpdateQuery();
            var result = await DalSession.Connection().QueryFirstAsync<T>(updateQry.Query, updateQry.Parameters, DalSession.Tasnsaction(), Timeout);
            return result;
        }

        public async Task<object> DeleteByIdAsync(object id)
        {
            var deleteQry = QueryBuilder.DeleteByIdQuery<T>(id);
            var result = await DalSession.Connection().ExecuteAsync(deleteQry.Query, deleteQry.Parameters, DalSession.Tasnsaction(), Timeout);
            return id;
        }

        public async Task<int> DeleteByQueryAsync(IQueryCollection keyValuePairs)
        {

            var selectObject = keyValuePairs.ToDictionary(x => x.Key, x => x.Value.ToString()).DeleteByQuery<T>();
            var result = await DalSession.Connection().ExecuteAsync(selectObject.Query, selectObject.Parameters, DalSession.Tasnsaction(), Timeout);
            return result;
        }
    }
}
