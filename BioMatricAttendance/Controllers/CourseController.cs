using BioMatricAttendance.DTOsModel;
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

        [HttpDelete("DeleteCourse/{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var result = await _courseService.DeleteCourseAsync(courseId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("GetAllCourse")]

        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpPut("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse( [FromBody] CourseDto courseDto)
        {
            var updatedCourse = await _courseService.UpdateCourseAsync(courseDto);
            if (updatedCourse == null)
            {
                return NotFound();
            }
            return Ok(updatedCourse);
        }

        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourse([FromBody] CourseDto courseDto)
        {
            var createdCourse = await _courseService.AddCourseAsync(courseDto);
            return Ok(createdCourse);
        }


    }
}
