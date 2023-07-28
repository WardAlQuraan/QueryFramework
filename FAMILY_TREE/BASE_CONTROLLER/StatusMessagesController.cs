using COMMON.API_RESPONSE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FAMILY_TREE.BASE_CONTROLLER
{
    [Route("[controller]")]
    [ApiController]
    public class StatusMessagesController : ControllerBase
    {
        public override OkObjectResult Ok([ActionResultObjectValue] object? value)
        {
            var result = new ApiRespone() { Data= value , StatusCode = 200 , IsError = false };
            return base.Ok(result);
        }

        [NonAction]
        public OkObjectResult Ok([ActionResultObjectValue] object? value , string message)
        {
            var result = new ApiRespone() { Data = value, StatusCode = 200, IsError = false , Message = message };
            return base.Ok(result);
        }

    }
}
