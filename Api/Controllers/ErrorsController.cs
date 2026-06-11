using API.Erorrs;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("Errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            if (code == 404)
            {
                return NotFound(new ApiResponse(code));
            }
            else if (code == 401)
            {
                return Unauthorized(401);
            }
            return StatusCode(code); 
        }
    }
}
