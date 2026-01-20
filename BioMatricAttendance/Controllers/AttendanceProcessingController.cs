using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceProcessingController : ControllerBase
    {
        private readonly IAttendanceProcessingService _attendanceProcessingService;
        public AttendanceProcessingController(IAttendanceProcessingService attendanceProcessingService)
        {
            _attendanceProcessingService = attendanceProcessingService;
        }
        [HttpGet("process")]
       public async Task<IActionResult> ProcessAttendance([FromQuery] DateTime startDate, DateTime endDate, int instituteId)
        {
            var logs=await _attendanceProcessingService.ProcessFacultyAttendanceAsync(startDate, endDate,instituteId);
            return Ok(new
            {
                Success = true,
                Message = "Attendance processing initiated successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = logs
            });
        }

        
    }
}
