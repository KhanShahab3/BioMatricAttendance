using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Response;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "super_admin")]
    public class SuperAdminDashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly AppDbContext _context;

        public SuperAdminDashboardController(IDashboardService dashboardService, AppDbContext context)
        {
            _dashboardService = dashboardService;
            _context = context;
        }
        [HttpGet("GetSuperAdminDashboard")]
        public async Task<IActionResult> GetSuperAdminDashboard([FromQuery] int regionId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {


            //var selectedDate = date??DateTime.UtcNow.Date;

            var result = await _dashboardService.GetSuperAdminDashboardAsync(regionId, startDate, endDate);
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
      

        [HttpGet("candidates")]
        public async Task<IActionResult> GetCandidates([FromQuery] int? instituteId, [FromQuery] int? regionId)
        {
           
           var result= await _dashboardService.GetCandidate(instituteId, regionId);

            return Ok(result);
        }

        [HttpPut("candidates/{id}/designation")]
        public async Task<IActionResult> UpdateCandidateDesignation(int id, [FromBody] string designation)
        {
            var candidate = await _context.Candidates.FindAsync(id);

            if (candidate == null)
            {
                return NotFound($"Candidate with ID {id} not found.");
            }

            
            candidate.Designation = designation; 

          
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Designation updated successfully successfully." });
        }


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
