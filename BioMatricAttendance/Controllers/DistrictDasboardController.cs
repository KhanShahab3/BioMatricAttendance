using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictDasboardController : ControllerBase
    {
        private readonly IDistrictDashboardServicecs _districtDashboardService;

        public DistrictDasboardController(IDistrictDashboardServicecs districtDashboardService)
        {
            _districtDashboardService = districtDashboardService;
        }
        [HttpGet("GetDistrictDashboard")]

        public async Task<IActionResult> GetDistrictDashboard(
            [FromQuery] int districtId,
            [FromQuery] int? instituteId)
        {
            var dashboard = await _districtDashboardService.GetDistrictDashboard(districtId, instituteId);
            return Ok(dashboard);

        }

        [HttpGet("GetDistrictDashboardReport")]
        public async Task<IActionResult> GetDistrictDashboardReport(
            [FromQuery] int districtId,

            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
             [FromQuery] int? instituteId=null
            )
        {
            var report = await _districtDashboardService.GetDashboardReportDto(districtId, startDate, endDate ,instituteId);
            return Ok(report);

        }
    }
}
