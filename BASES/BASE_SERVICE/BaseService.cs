using BASES.BASE_REPO;
using COMMON.BUILDER;
using CONNECTION_FACTORY.DAL_SESSION;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASES.BASE_SERVICE
{
    public class BaseService<T>:IBaseService<T>
    {
        protected IDalSession dalSession;
        protected BaseRepo<T> _repo;
        public BaseService(IDalSession dalSession) 
        { 
            this.dalSession = dalSession; 
        }

        public async Task<IEnumerable<T>> GetAsync(IQueryCollection keyValuePairs)
        {
            dalSession.Begin();
            _repo = new BaseRepo<T>(dalSession);
            var res = await _repo.GetAsync(keyValuePairs);
            return res;
        }

        public async Task<T> GetAsync(object id)
        {
            dalSession.Begin();
            _repo = new BaseRepo<T>(dalSession);
            var res = await _repo.GetAsync(id);
            return res;
        }

        public async Task<object> InsertAsync(T entity)
        {
            try
            {
                dalSession.Begin();
                var repo = new BaseRepo<T>(dalSession);
                var res = await repo.InsertAsync(entity);
                dalSession.Commit();
                return res;
            }
            catch (Exception ex)
            {
                dalSession.Rollback();
                throw;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                dalSession.Begin();
                var repo = new BaseRepo<T>(dalSession);
                var res = await repo.UpdateAsync(entity);
                dalSession.Commit();
                return res;
            }
            catch (Exception ex)
            {
                dalSession.Rollback();
                throw;
            }
        }

        public async Task<object> DeleteByIdAsync(object id)
        {
            try
            {
                dalSession.Begin();
                var repo = new BaseRepo<T>(dalSession);
                var res = await repo.DeleteByIdAsync(id);
                dalSession.Commit();
                return res;
            }
            catch (Exception ex)
            {
                dalSession.Rollback();
                throw;
            }
        }

        public async Task<int> DeleteByQueryAsync(IQueryCollection keyValuePairs)
        {

            try
            {
                dalSession.Begin();
                var repo = new BaseRepo<T>(dalSession);
                var res = await repo.DeleteByQueryAsync(keyValuePairs);
                dalSession.Commit();
                return res;
            }
            catch (Exception ex)
            {
                dalSession.Rollback();
                throw;
            }
        }
    }
}
