using BASES.BASE_REPO;
using BASES.BASE_SERVICE;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES;
using Microsoft.AspNetCore.Mvc;

namespace FAMILY_TREE.BASE_CONTROLLER
{
    public class BaseController<T,I> : StatusMessagesController
    {
        private readonly IBaseService<T,I> _service;
        public BaseController(IBaseService<T, I> baseService)
        {
            _service = baseService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            
            var res = await _service.GetAsync(Request.Query);
            return Ok(res);

        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(int id)
        {
            var res = await _service.GetAsync(id);
            return Ok(res);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post(T entity)
        {
            var res = await _service.InsertAsync(entity);
            return Ok(res);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put(T entity)
        {
            var res = await _service.UpdateAsync(entity);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var res = await _service.DeleteByIdAsync(id);
            return Ok(res);
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete()
        {
            var res = await _service.DeleteByQueryAsync(Request.Query);
            return Ok(res);
        }
    }
}
