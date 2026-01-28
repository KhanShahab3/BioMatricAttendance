using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService; 

        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }
        [HttpGet("GetDistricts")]
        public async Task<IActionResult> GetDistricts()
        {
            var districts = await _districtService.GetDistrict();
            return Ok(districts);
        }
    }
}
