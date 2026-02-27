using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "district_dashboard")]
    public class DistrictDasboardController : ControllerBase
    {
        private readonly IDistrictDashboardServicecs _districtDashboardService;

        public DistrictDasboardController(IDistrictDashboardServicecs districtDashboardService)
        {
            _districtDashboardService = districtDashboardService;
        }
        [HttpGet("GetDistrictDashboard")]

        public async Task<IActionResult> GetDistrictDashboard(
        
            [FromQuery] int? instituteId)
        {
            int ? districtId=null;

            var districtClaim = User.FindFirst("DistrictId")?.Value;
            if (!string.IsNullOrEmpty(districtClaim))
                districtId = int.Parse(districtClaim);

            var dashboard = await _districtDashboardService.GetDistrictDashboard(districtId, instituteId);
            return Ok(dashboard);

        }

        [HttpGet("GetDistrictDashboardReport")]
        public async Task<IActionResult> GetDistrictDashboardReport(
            
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
             [FromQuery] int? instituteId
            )
        {
            int? districtId = null;
            var districtClaim = User.FindFirst("DistrictId")?.Value;
            if (!string.IsNullOrEmpty(districtClaim))
                districtId = int.Parse(districtClaim);
            var report = await _districtDashboardService.GetDashboardReportDto(districtId, startDate, endDate ,instituteId);
            return Ok(report);

        }
    }
}
