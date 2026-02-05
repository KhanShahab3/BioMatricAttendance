using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        public ShiftController(IShiftService shiftService) {
        
        _shiftService = shiftService;
        }
        [HttpPost("assign")]
        public async Task<IActionResult> AssignShift( AssignShiftDto dto)
        {
            if (dto == null || !dto.CandidateIds.Any())
                return BadRequest("No candidates selected");

            await _shiftService.AssignShiftAsync(dto);
            return Ok(new { Message = "Shifts assigned successfully" });
        }
    }
}
