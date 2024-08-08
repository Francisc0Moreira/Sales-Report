using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIOLoadDataService;

namespace SIODashboard.Server.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        [HttpPost("RunSaft")]
        public IActionResult Saft([FromForm] IFormFile file)
        {
            try
            {
                string result = RunSaft.ExecuteSaft(file);
                return Ok(new { success = true, message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
