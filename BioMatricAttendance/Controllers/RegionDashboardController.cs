using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionDashboardController : ControllerBase
    {
        private readonly IRegionDashboardService _regionDashboardService;   

        public RegionDashboardController(IRegionDashboardService regionDashboardService)
        {
            _regionDashboardService = regionDashboardService;
        }
        [HttpGet("GetRegionDashboard")]
        public async Task<IActionResult> GetRegionDashboard(
            [FromQuery] int regionId,
            [FromQuery] int? districtId)
            
        {
            var dashboard = await _regionDashboardService.GetRegionDashboardAsync(regionId, districtId);
            return Ok(dashboard);
        }
    }
}
