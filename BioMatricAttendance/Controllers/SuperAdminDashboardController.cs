using BioMatricAttendance.Response;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "super_admin")]
    public class SuperAdminDashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public SuperAdminDashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("GetSuperAdminDashboard")]
        public async Task<IActionResult> GetSuperAdminDashboard([FromQuery] int regionId,[FromQuery]DateTime? startDate,[FromQuery]DateTime? endDate)
        {


            //var selectedDate = date??DateTime.UtcNow.Date;
            
            var result = await _dashboardService.GetSuperAdminDashboardAsync(regionId,  startDate,  endDate);
            return Ok(new APIResponse<object>
            {
                sucess = true,
                Message = "Super Admin Dashboard Data Retrieved sucessfully",
                StatusCode = StatusCodes.Status200OK,
                Data = result
            });
        }
        [HttpGet("GetAttendanceReport")]
        public async Task<IActionResult> GetAttendanceReport([FromQuery] int? regionId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var result = await _dashboardService.GetAttendanceReportAsync(regionId, startDate, endDate);
            return Ok(new APIResponse<object>
            {
                sucess = true,
                Message = "Attendance Report Data Retrieved sucessfully",
                StatusCode = StatusCodes.Status200OK,
                Data = result
            });
        }
        //[HttpGet("InstituteTable")]
        //public async Task<IActionResult> InstituteTable([FromQuery] int? regionId)
        //{
        //    var result = await _dashboardService.InstituteTableAsync(regionId);
        //    return Ok(new APIResponse<object>
        //    {
        //        sucess = true,
        //        Message = "Institute Table Data Retrieved sucessfully",
        //        StatusCode = StatusCodes.Status200OK,
        //        Data = result
        //    });
        //}

    }
}
