using BASES.BASE_REPO;
using BASES.BASE_SERVICE;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES;
using Microsoft.AspNetCore.Mvc;

namespace FAMILY_TREE.BASE_CONTROLLER
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController<T> : ControllerBase
    {
        private readonly IBaseService<T> _service;
        public BaseController(IBaseService<T> baseService)
        {
            _service = baseService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var res = await _service.GetAsync(Request.Query);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var res = await _service.GetAsync(id);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(T entity)
        {
            try
            {
                var res = await _service.InsertAsync(entity);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(T entity)
        {
            try
            {
                var res = await _service.UpdateAsync(entity);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var res = await _service.DeleteByIdAsync(id);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteByQuery()
        {
            try
            {
                var res = await _service.DeleteByQueryAsync(Request.Query);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
