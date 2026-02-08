using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "institute_admin")]
    public class InstituteDashboardController : ControllerBase
    {
        private readonly IInstituteDashboardService _instituteDashboardService;
        public InstituteDashboardController(IInstituteDashboardService instituteDashboardService)
        {
            _instituteDashboardService = instituteDashboardService;
        }

   

        [HttpGet("Institute/dashboard")]
        public async Task<IActionResult> GetInstituteDashboard()
        {
            int? instituteId = null;
         

            var instituteClaim = User.FindFirst("InstituteId")?.Value;
            if (!string.IsNullOrEmpty(instituteClaim))
                instituteId = int.Parse(instituteClaim);

            //var regionClaim = User.FindFirst("RegionId")?.Value;
            //if (!string.IsNullOrEmpty(regionClaim))
            //    regionId = int.Parse(regionClaim);

            var dashboard = await _instituteDashboardService.GetInstituteDashboard(instituteId);

            return Ok(dashboard);
        }

        [HttpGet("Institute/Course")]

        
        public async Task<IActionResult>GetInstituteCourse()

        {

            int? instituteId = null;
            //int? regionId = null;

            var instituteClaim = User.FindFirst("InstituteId")?.Value;
            if (!string.IsNullOrEmpty(instituteClaim))
                instituteId = int.Parse(instituteClaim);
            var courses = await _instituteDashboardService.GetCourseWiseAttendanceAsync(instituteId);
            return Ok(courses);

        }

        [HttpGet("Institute/AttendanceReports")]

        public async Task<IActionResult>GetInstituteAttendanceReports([FromQuery] int instituteId, [FromQuery] DateTime ?startDate, [FromQuery] DateTime? endDate)

        {
           


            var instituteAttendance=await _instituteDashboardService.GetAttendanceReportAsync(instituteId, startDate, endDate);
            return Ok(instituteAttendance);
        }
    }
}
