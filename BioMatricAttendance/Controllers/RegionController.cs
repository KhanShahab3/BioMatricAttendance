using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet("allRegionName")]

        public async Task<IActionResult> GetAllRegionNames()
        {
            var regionNames = await _regionService.GetRegions();
            if (regionNames.Count == 0)
            {
                return NotFound(new
                {
                    Sucess = false,
                    Message = "No region names found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new
            {
                Sucess = true,
                Message = "Region names fetched successfully",
                Data = regionNames,
                StatusCode = 200
            });
        }
    }

}
