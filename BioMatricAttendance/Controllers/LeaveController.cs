using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase



    {

        private readonly ILeaveManagmentService _leaveManagmentService;
        public LeaveController(ILeaveManagmentService leaveManagmentService)
        {
            _leaveManagmentService = leaveManagmentService;
        }
        [HttpGet("absent")]
        public async Task<IActionResult> GetAbsentCandidates([FromQuery]int regionId,  [FromQuery] int instituteId,DateTime startDate,DateTime endDate)
        {
            var result = await _leaveManagmentService.GetAbsentCandidates(regionId,instituteId,startDate,endDate);
            return Ok(result);
        }

     
        [HttpPost("assign")]
        public async Task<IActionResult> AssignLeave( AssignLeaveDto Dto)
        {
            await _leaveManagmentService.AssignLeave(
          Dto
                //Dto.AssignedBy
            );

            return Ok(new { message = "Leave assigned successfully" });
        }
    }
}

