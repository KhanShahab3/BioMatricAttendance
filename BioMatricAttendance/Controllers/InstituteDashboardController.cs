using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstituteDashboardController : ControllerBase
    {
        private readonly IInstituteDashboardService _instituteDashboardService;
        public InstituteDashboardController(IInstituteDashboardService instituteDashboardService)
        {
            _instituteDashboardService = instituteDashboardService;
        }

        [HttpGet("Institute/dashboard/{instituteId}")]
        public async Task<IActionResult> GetInstituteDashboard(int instituteId)
        {
            var dashboard = await _instituteDashboardService.GetInstituteDashboard(instituteId);

            return Ok(dashboard);
        }

        [HttpGet("Institute/Course/{instituteId}")]

        public async Task<IActionResult>GetInstituteCourse(int instituteId)
        {
            var courses = await _instituteDashboardService.GetCourseWiseAttendanceAsync(instituteId);
            return Ok(courses);

        }

        [HttpGet("Institute/AttendanceReports")]

        public async Task<IActionResult>GetInstituteAttendanceReports([FromQuery] int instituteId, [FromQuery] DateTime from, [FromQuery] DateTime to)

        {
           


            var instituteAttendance=await _instituteDashboardService.GetAttendanceReportAsync(instituteId, from, to);
            return Ok(instituteAttendance);
        }
    }
}
