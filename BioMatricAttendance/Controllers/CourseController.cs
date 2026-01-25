using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpGet("CourseByInstitute/{instituteId}")]
        public async Task<IActionResult> GetCoursesByInstitute(int instituteId)
        {
            var courses = await _courseService.GetCoursesByInstituteId(instituteId);
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }
    }
}
