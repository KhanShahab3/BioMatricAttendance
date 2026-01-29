using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Response;
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
        private readonly ICourseCandidateService _courseCandidateService;
        
        public CourseController(ICourseService courseService,ICourseCandidateService courseCandidateService)
        {
            _courseService = courseService;
            _courseCandidateService = courseCandidateService;
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
        [HttpGet("GetCourseCandidate")]

        public async Task<IActionResult> GetCourseCandidate(int courseId)
        {
            var courseInfo = await _courseCandidateService.GetCouseInfoAsync(courseId);
            if (courseInfo==null)
            {
                return NotFound(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "No CourseInfo found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "CourseInfo fetched successfully",
                Data = courseInfo,
                StatusCode = 200
            });
            
        }
        [HttpPost("AddCourseCandidate")]
        public async Task<IActionResult> AddCourseCandidate(CourseCandidateDto dto)
        {
            await _courseCandidateService.AddCourseCandidates(dto);
            return Ok();
        }

        [HttpPost("RemoveCourseCandidate")]
        public async Task<IActionResult> AddCourseCandidate(int id)
        {
           var res= await _courseCandidateService.GetById(id);

            if (res == null)
            {
                return NotFound();
            }

            await _courseCandidateService.RemoveCourseCandidates(res);
            return Ok();
        }


    }
}
