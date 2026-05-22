using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

       [HttpGet]
      
            public IActionResult Get()
            {
                return Ok("Working");
            }
        
    }
}
