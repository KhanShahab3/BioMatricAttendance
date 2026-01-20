using BioMatricAttendance.Response;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminDashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public SuperAdminDashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("GetSuperAdminDashboard")]
        public async Task<IActionResult> GetSuperAdminDashboard([FromQuery] int regionId)
        {


            //var selectedDate = date??DateTime.UtcNow.Date;
            
            var result = await _dashboardService.GetSuperAdminDashboardAsync(regionId);
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Super Admin Dashboard Data Retrieved Successfully",
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
        //        Sucess = true,
        //        Message = "Institute Table Data Retrieved Successfully",
        //        StatusCode = StatusCodes.Status200OK,
        //        Data = result
        //    });
        //}

    }
}
