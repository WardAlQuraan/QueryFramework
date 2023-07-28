using ENTITIES.VIEW_MODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERVICES.AUTH;
using SERVICES.TEST_ENTITY;

namespace FAMILY_TREE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthVM auth)
        {
            try
            {
                var res = await _service.Login(auth); 
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
