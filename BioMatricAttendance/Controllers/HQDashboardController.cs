using BioMatricAttendance.Response;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Authorize(Roles = "hq_admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class HQDashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public HQDashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("hq/dashboard")]

        public async Task<IActionResult> GetHQDashboard([FromQuery] int? regionId)
        {
            var result = await _dashboardService.GetSuperAdminDashboardAsync(regionId ?? 0);
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "HQ Dashboard Data Retrieved Successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}
