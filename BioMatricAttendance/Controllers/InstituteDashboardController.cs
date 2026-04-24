using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    
    [Authorize(Roles = "super_admin,institute_admin")]
    public class InstituteDashboardController : ControllerBase
    {
        private readonly IInstituteDashboardService _instituteDashboardService;
        public InstituteDashboardController(IInstituteDashboardService instituteDashboardService)
        {
            _instituteDashboardService = instituteDashboardService;
        }

        [HttpGet("Institute/dashboard")]
      
        public async Task<IActionResult> GetInstituteDashboard(int? id)
        {
            int? instituteId = null;

          
            var instituteClaim = User.FindFirst("InstituteId")?.Value;
            if (!string.IsNullOrEmpty(instituteClaim))
            {
                instituteId = int.Parse(instituteClaim);
            }

           
            if (User.IsInRole("super_admin"))
            {
                instituteId = id;
            }

            var dashboard = await _instituteDashboardService.GetInstituteDashboard(instituteId);
            return Ok(dashboard);
        }

        [HttpGet("Institute/Course")]
        public async Task<IActionResult> GetInstituteCourse(int? id)
        {
            int? instituteId = null;

            // Keep your original claim check
            var instituteClaim = User.FindFirst("InstituteId")?.Value;
            if (!string.IsNullOrEmpty(instituteClaim))
                instituteId = int.Parse(instituteClaim);

            // Add the Super Admin override
            if (User.IsInRole("super_admin"))
                instituteId = id;

            var courses = await _instituteDashboardService.GetCourseWiseAttendanceAsync(instituteId);
            return Ok(courses);
        }

        [HttpGet("Institute/AttendanceReports")]
        public async Task<IActionResult> GetInstituteAttendanceReports(int? id, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            int? instituteId = null;

            var instituteClaim = User.FindFirst("InstituteId")?.Value;
            if (!string.IsNullOrEmpty(instituteClaim))
                instituteId = int.Parse(instituteClaim);

            if (User.IsInRole("super_admin"))
                instituteId = id;

            var instituteAttendance = await _instituteDashboardService.GetAttendanceReportAsync(instituteId, startDate, endDate);
            return Ok(instituteAttendance);
        }
    }
}