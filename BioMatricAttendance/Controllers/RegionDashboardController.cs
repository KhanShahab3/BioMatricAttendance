using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Authorize(Roles ="region_admin")]
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
            //[FromQuery] int regionId,
            [FromQuery] int? districtId)
            
        {
            int ?regionId = null;
            var regionClaim = User.FindFirst("RegionId")?.Value;
            if (!string.IsNullOrEmpty(regionClaim))
                regionId = int.Parse(regionClaim);

            var dashboard = await _regionDashboardService.GetRegionDashboardAsync(regionId,districtId);
            return Ok(dashboard);
        }

        [HttpGet("GetRegionalDashboardReport")]
        public async Task<IActionResult> GetRegionalDashboardReport(
          
            [FromQuery] int? districtId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            int regionId = 0;
            var regionClaim = User.FindFirst("RegionId")?.Value;
            if (!string.IsNullOrEmpty(regionClaim))
                regionId = int.Parse(regionClaim);

            var report = await _regionDashboardService.GetRegionalDashboardAsync(regionId, districtId, startDate, endDate);
            return Ok(report);
        }
    }
}
